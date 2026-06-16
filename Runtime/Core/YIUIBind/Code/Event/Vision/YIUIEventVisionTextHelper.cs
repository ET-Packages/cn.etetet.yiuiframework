#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;

#if TextMeshPro
using TMPro;
#endif

namespace YIUIFramework
{
    internal static class YIUIEventVisionTextHelper
    {
        internal static T ResolveComponent<T>(Component owner, T serialized) where T : Component
        {
            if (serialized != null)
            {
                return serialized;
            }

            if (owner == null)
            {
                return null;
            }

            return owner.GetComponent<T>();
        }

        internal static string FormatSelectable(Selectable selectable)
        {
            if (selectable == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(selectable) + "，interactable=" + YIUIVisionTextHelper.FormatBool(selectable.interactable);
        }

        internal static string FormatInputField(InputField inputField)
        {
            if (inputField == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(inputField) + "，text=" + YIUIVisionTextHelper.Quote(inputField.text) + "，interactable=" + YIUIVisionTextHelper.FormatBool(inputField.interactable);
        }

        internal static string FormatDropdown(Dropdown dropdown)
        {
            if (dropdown == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(dropdown) + "，value=" + dropdown.value + "，options=" + dropdown.options.Count + "，interactable=" + YIUIVisionTextHelper.FormatBool(dropdown.interactable);
        }

        internal static string FormatSlider(Slider slider)
        {
            if (slider == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(slider) + "，value=" + YIUIVisionTextHelper.FormatFloat(slider.value) + "，range=[" + YIUIVisionTextHelper.FormatFloat(slider.minValue) + ", " + YIUIVisionTextHelper.FormatFloat(slider.maxValue) + "]，interactable=" + YIUIVisionTextHelper.FormatBool(slider.interactable);
        }

        internal static string FormatScrollbar(Scrollbar scrollbar)
        {
            if (scrollbar == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(scrollbar) + "，value=" + YIUIVisionTextHelper.FormatFloat(scrollbar.value) + "，size=" + YIUIVisionTextHelper.FormatFloat(scrollbar.size) + "，interactable=" + YIUIVisionTextHelper.FormatBool(scrollbar.interactable);
        }

        internal static string FormatToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(toggle) + "，isOn=" + YIUIVisionTextHelper.FormatBool(toggle.isOn) + "，interactable=" + YIUIVisionTextHelper.FormatBool(toggle.interactable);
        }

        #if TextMeshPro
        internal static string FormatTMPInputField(TMP_InputField inputField)
        {
            if (inputField == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(inputField) + "，text=" + YIUIVisionTextHelper.Quote(inputField.text) + "，interactable=" + YIUIVisionTextHelper.FormatBool(inputField.interactable);
        }

        internal static string FormatTMPDropdown(TMP_Dropdown dropdown)
        {
            if (dropdown == null)
            {
                return "null";
            }

            return YIUIVisionTextHelper.FormatUnityObject(dropdown) + "，value=" + dropdown.value + "，options=" + dropdown.options.Count + "，interactable=" + YIUIVisionTextHelper.FormatBool(dropdown.interactable);
        }
        #endif
    }
}

#endif
