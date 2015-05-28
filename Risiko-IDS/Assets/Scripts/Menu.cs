﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    private Animator animator;
    private CanvasGroup canvasGroup;

	public bool IsOpen
    {
        get
        {
            return animator.GetBool("IsOpen");
        }
        set
        {
            animator.SetBool("IsOpen", value);
        }
    }
	void Awake() 
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
	}
	
	
	
}
