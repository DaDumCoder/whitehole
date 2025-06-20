﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JetSystems
{

    public class AnimatedElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public LeanTweenType tweenType;
        public float duration;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LeanTween.scale(gameObject, Vector3.one * 0.9f, duration).setEase(tweenType);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, Vector3.one, duration).setEase(tweenType);
        }
    }
}