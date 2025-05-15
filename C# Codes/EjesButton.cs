using UnityEngine;
using UnityEngine.UI;

public class EjesButton : MonoBehaviour
{
    [SerializeField] private Button ButtonAxis;
    [SerializeField] private Animator animator;

    // Definimos 3 colores diferentes
    [SerializeField] private Color color1 = Color.red;
    [SerializeField] private Color color2 = Color.green;
    [SerializeField] private Color color3 = Color.white;
    [SerializeField] private GameObject[] objects; // eje 1 ... 6
    [SerializeField] private Slider sliderVerificator;


    private int n = 0;

    void Start()
    {
        ButtonAxis.onClick.AddListener(CheckAnimation);

        // Asegurarnos que tenemos los componentes necesarios
        if (animator == null)
            animator = GetComponent<Animator>();

        if (ButtonAxis == null)
            ButtonAxis = GetComponent<Button>();
    }

    public void CheckAnimation()
    {
        int ejeChange=(int) sliderVerificator.value;
        switch (n)
        {
            case 0:
                ButtonAxis.image.color = color1;
                animator.SetTrigger("FirstSelect");
                Debug.Log("Activando color rojo y animación FirstSelect");
                ShowAllObjects(false);


                break;

            case 1:
                ButtonAxis.image.color = color2;
                animator.SetTrigger("SecondSelect");
                Debug.Log("Activando color verde y animación SecondSelect");
                
                objects[ejeChange].SetActive(true);

                break;

            case 2:
                ButtonAxis.image.color = color3;
                animator.SetTrigger("Reset");
                Debug.Log("Activando color azul y animación Reset");
                n = -1; // Lo ponemos en -1 porque luego se incrementará a 0
                ShowAllObjects(true);


                break;

        }

        n++;
        Debug.Log($"Estado actual: {n}");
        
    }
    public void ShowAllObjects(bool val)
    {
        if (objects == null || objects.Length == 0)
        {
            Debug.LogWarning("El array 'objects' está vacío o no asignado.");
            return;
        }

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(val); // Activa cada objeto
            }
        }
        Debug.Log("Todos los objetos han sido mostrados.");
    }
}
