using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Interpolation;

namespace AnimatedUI
{
    public enum TextPropertyIndex
    {
        CharacterSpacing = CommonProperty.MAX + 0,
        FontSize = CommonProperty.MAX + 1,
        //HorizontalAlignment = CommonProperty.MAX + 2,
        //VerticalAlignment = CommonProperty.MAX + 3,
    }

    public enum TextFlag
    {
        UseDefaultCharacterSpacing = CommonFlag.MAX + 0,
        UseDefaultFontSize = CommonFlag.MAX + 1,
        //UseDefaultAlignment = CommonFlag.MAX + 2,
    }

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatedTMPro : AnimatedUIElement
    {
        protected TextMeshProUGUI tmp;

        // static variables
        static public int[] horizontalAlignmentValues = (int[])Enum.GetValues(typeof(HorizontalAlignmentOptions));
        static public int[] verticalAlignmentValues = (int[])Enum.GetValues(typeof(VerticalAlignmentOptions));
        protected override void ResetContent()
        {
            if(!tmp) tmp = GetComponent<TextMeshProUGUI>();
        }

        protected override void SetAdditionalProperties(ref List<Property> properties)
        {
        //  properties.Add(new Property(VALUE)); // "PROPERTY TEMPLATE"
            properties.Add(new Property(TextPropertyIndex.CharacterSpacing.ToString(),tmp.characterSpacing)); // "CharacterSpacing"
            properties.Add(new Property(TextPropertyIndex.FontSize.ToString(),tmp.fontSize)); // "FontSize"
            //properties.Add(new Property(TextPropertyIndex.HorizontalAlignment.ToString(),(int)tmp.horizontalAlignment)); // "HorizontalAlignment"
            //properties.Add(new Property(TextPropertyIndex.VerticalAlignment.ToString(),(int)tmp.verticalAlignment)); // "VerticalAlignment"
        }

        protected override void SetAdditionalFlags(ref List<Flag> flags,bool defaultFlag)
        {
        //  flags.Add(DEFAULTFLAG); // "FLAG TEMPLATE"
            flags.Add(new Flag(TextFlag.UseDefaultCharacterSpacing.ToString(),defaultFlag)); // "UseDefaultCharacterSpacing"
            flags.Add(new Flag(TextFlag.UseDefaultFontSize.ToString(),defaultFlag)); // "UseDefaultFontSize"
            //flags.Add(new Flag(TextFlag.UseDefaultAlignment.ToString(),defaultFlag)); // "UseDefaultAlignment"
        }

        protected override void OnValidateContent(StateInfo info)
        {
            if(!tmp) tmp = GetComponent<TextMeshProUGUI>();
            if(!tmp) return;
            if(info.flags[(int)CommonFlag.UseDefaultColor].IsChecked) info.properties[(int)CommonProperty.Color].Color = tmp.color;
            if(info.flags[(int)TextFlag.UseDefaultCharacterSpacing].IsChecked) info.properties[(int)TextPropertyIndex.CharacterSpacing].Float = tmp.characterSpacing;
            if(info.flags[(int)TextFlag.UseDefaultFontSize].IsChecked) info.properties[(int)TextPropertyIndex.FontSize].Float = tmp.fontSize;
            //if(info.flags[(int)TextFlag.UseDefaultAlignment].IsChecked) info.properties[(int)TextPropertyIndex.HorizontalAlignment].Integer = (int)tmp.horizontalAlignment;
            //if(info.flags[(int)TextFlag.UseDefaultAlignment].IsChecked) info.properties[(int)TextPropertyIndex.VerticalAlignment].Integer = (int)tmp.verticalAlignment;
        }
        
        protected override void ApplyAdditionalValues()
        {
            tmp.color = currentProperties[(int)CommonProperty.Color].Vector;
            tmp.characterSpacing = currentProperties[(int)TextPropertyIndex.CharacterSpacing].Float;
            tmp.fontSize = currentProperties[(int)TextPropertyIndex.FontSize].Float;
            //tmp.horizontalAlignment = (HorizontalAlignmentOptions)currentProperties[(int)TextPropertyIndex.HorizontalAlignment].Integer;
            //tmp.verticalAlignment = (VerticalAlignmentOptions)currentProperties[(int)TextPropertyIndex.VerticalAlignment].Integer;
        }

        protected override void InitializeContent()
        {

        }

        protected override void UninitializeContent()
        {
            
        }

        protected override void StartTransitContent(int targetIndex)
        {
            
        }

        protected override void InterruptContent(int targetIndex)
        {
            
        }

        protected override void EndTransitContent()
        {
            
        }

        protected override void TransitContent(float ratio)
        {
            
        }

        void ValidateHorizontalAlignment(ref int value,bool next = false)
        {
            for(int index = 0,max = horizontalAlignmentValues.Length;index < max;index++)
            {
                if(index == horizontalAlignmentValues[index])   return;
            }
            for(int index = 0,max = horizontalAlignmentValues.Length;index < max;index++)
            {
                if(index == horizontalAlignmentValues[index])   return;
            }
        }

        void ValidateVerticalAlignment(ref int value,bool next = false)
        {
            for(int index = 0,max = verticalAlignmentValues.Length;index < max;index++)
            {
                if(index == verticalAlignmentValues[index]) return;
            }
            for(int index = 0,max = horizontalAlignmentValues.Length;index < max;index++)
            {
                if(index == horizontalAlignmentValues[index])   return;
            }
        }
    }

}
