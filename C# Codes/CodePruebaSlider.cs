using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CodePruebaSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText = null;
    public GameObject[] objects; // Arreglo de objetos J1, J2, etc.

    private GameObject currentSelectedJoint;
    private int previousIndex = -1;

    // Color for the outline
    private Color outlineColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

    // Dictionary to store original material colors
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void Start()
    {
        // Store original material colors for all objects
        foreach (GameObject obj in objects)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                originalColors[obj] = renderer.material.color;
            }
        }

        // Initialize with current slider value
        if (slider != null)
        {
            Change(slider.value);
        }
    }

    public void Change(float value)
    {
        if (sliderText != null)
        {
            sliderText.text = value.ToString("0");
        }

        int jointIndex = (int)value - 1; // Convert 1-6 to 0-5 for array indexing

        // First restore all objects to their original state
        RestoreAllMaterials();

        // Check if the index is valid
        if (jointIndex >= 0 && jointIndex < objects.Length)
        {
            currentSelectedJoint = objects[jointIndex];

            // Add highlight to the selected joint
            if (currentSelectedJoint != null)
            {
                var renderer = currentSelectedJoint.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Use emission to simulate an outline
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", outlineColor);
                }
            }

            previousIndex = jointIndex;
        }
        else
        {
            Debug.LogWarning("Invalid joint index: " + jointIndex);
        }
    }

    private void RestoreAllMaterials()
    {
        // Reset emission on all objects
        foreach (GameObject obj in objects)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.DisableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.black);

                // Restore original color if we have it stored
                if (originalColors.ContainsKey(obj))
                {
                    renderer.material.color = originalColors[obj];
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Make sure we restore materials when script is destroyed
        RestoreAllMaterials();
    }
}