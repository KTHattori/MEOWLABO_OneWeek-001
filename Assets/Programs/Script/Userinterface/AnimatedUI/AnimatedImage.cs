using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

namespace AnimatedUI
{
    public enum ImageProperty
    {
        FillAmount = CommonProperty.MAX + 0,
        FillMethod = CommonProperty.MAX + 1,
    }

    public enum ImageFlag
    {
        Image_UseDefualtFillAmount = CommonFlag.MAX + 0,
        Image_UseDefualtFillMethod = CommonFlag.MAX + 1,
    }

    [RequireComponent(typeof(Image))]
    public class AnimatedImage : AnimatedUIElement
    {
        protected Image m_image;
        public Image image {get {return m_image;}}
        protected override void ResetContent()
        {
            if(!m_image) m_image = GetComponent<Image>();
        }

        protected override void SetAdditionalProperties(ref List<Property> properties)
        {
        //  properties.Add(new Property(VALUE)); // "PROPERTY TEMPLATE"
            properties.Add(new Property(ImageProperty.FillAmount.ToString(),0.0f)); // "Image FillAmount"
            properties.Add(new Property(ImageProperty.FillMethod.ToString(),(int)m_image.fillMethod));    // "Image FillMethod"
        }

        protected override void SetAdditionalFlags(ref List<Flag> flags,bool defaultFlag)
        {
        //  flags.Add(DEFAULTFLAG); // "FLAG TEMPLATE"
            flags.Add(new Flag(ImageFlag.Image_UseDefualtFillAmount.ToString(),defaultFlag)); // "Use Default Image FillAmount"
            flags.Add(new Flag(ImageFlag.Image_UseDefualtFillMethod.ToString(),defaultFlag)); // "Use Default Image FillMethod"
        }

        protected override void OnValidateContent(StateInfo info)
        {
            if(!m_image) m_image = GetComponent<Image>();
            if(!m_image) return;
            if(info.flags[(int)CommonFlag.UseDefaultColor].IsChecked) info.properties[(int)CommonProperty.Color].Color = m_image.color;
            if(info.flags[(int)ImageFlag.Image_UseDefualtFillAmount].IsChecked) info.properties[(int)ImageProperty.FillAmount].Float = m_image.fillAmount;
            if(info.flags[(int)ImageFlag.Image_UseDefualtFillMethod].IsChecked) info.properties[(int)ImageProperty.FillMethod].Integer = (int)m_image.fillMethod;
        }
        
        protected override void ApplyAdditionalValues()
        {
            m_image.color = currentProperties[(int)CommonProperty.Color].Vector;
            m_image.fillAmount = Mathf.Clamp01(currentProperties[(int)ImageProperty.FillAmount].Float);
        }

        protected override void InitializeContent()
        {
            m_image.type = Image.Type.Filled;
        }

        protected override void UninitializeContent()
        {
            
        }

        protected override void StartTransitContent(int targetIndex)
        {
            m_image.fillMethod = (Image.FillMethod)stateInfoDict[targetIndex].properties[(int)ImageProperty.FillMethod].Integer;
        }

        protected override void InterruptContent(int targetIndex)
        {
            m_image.fillMethod = (Image.FillMethod)stateInfoDict[targetIndex].properties[(int)ImageProperty.FillMethod].Integer;
        }

        protected override void EndTransitContent()
        {
            
        }

        protected override void TransitContent(float ratio)
        {
            
        }
    }

}