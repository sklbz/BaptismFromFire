using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fadeImage; // Un élément Image pour le fondu
  

     public GameObject title;
     public Vector3 titleAnimationStartPos, titleAnimationEndPos, spaceAnimationStartPos, spaceAnimationEndPos;
     public GameObject musicObject;
     public AudioSource audioSource;
      public float fadeDuration = 5.0f; // Durée du fondu
     string sceneName = "GameScene";
    void Start()
    {
        
          
          float t = 0.0f; // Valeur de temps initiale
          float animationDuration = 7.0f; // Durée de l'animation en secondes

          while (t < 1.0f)
          {
            t += Time.deltaTime / animationDuration; // Incrémentez t en fonction du temps
            title.transform.position = Vector3.Lerp(titleAnimationStartPos, titleAnimationEndPos, t);
          }
          audioSource.Play();
    }






    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {

         
    
            // Commencez par lancer la coroutine de fondu au démarrage de la scène
            title.GetComponent<Animator>().Play("Transition");
            StartCoroutine(PerformSceneTransition("SampleScene"));
    
            

        }


       

        IEnumerator FadeOut()
        {
            float timer = 0f;
            Color startColor = new Color(0, 0, 0, 0); // Alpha de 0 (transparent)
            Color endColor = new Color(0, 0, 0, 1); // Alpha de 1 (noir)

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
                yield return null;
            }
        }

        IEnumerator FadeIn()
        {
            float timer = 0f;
            Color startColor = new Color(0, 0, 0, 1); // Alpha de 1 (noir)
            Color endColor = new Color(0, 0, 0, 0); // Alpha de 0 (transparent)

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
                
                yield return null;
            }
        }
        IEnumerator PerformSceneTransition(string sceneName)
          {
                // Réduisez le volume de la musique de fond
                StartCoroutine(FadeOutMusic());

                // Fondu sortant
                yield return StartCoroutine(FadeOut());

                // Chargez la nouvelle scène
                SceneManager.LoadScene(sceneName);

                // Fondu entrant
                yield return StartCoroutine(FadeIn());

                // Rétablissez le volume de la musique de fond
                StartCoroutine(FadeInMusic());
          }




        IEnumerator FadeOutMusic()
        {
            float startVolume = audioSource.volume;
            float targetVolume = 0.0f;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeDuration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

        IEnumerator FadeInMusic()
        {
            float startVolume = audioSource.volume;
            float targetVolume = 1.0f;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeDuration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }
}
}