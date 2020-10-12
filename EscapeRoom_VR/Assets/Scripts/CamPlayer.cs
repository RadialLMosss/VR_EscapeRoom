﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayer : MonoBehaviour
{

    private Ray ray;
    private RaycastHit hit;
    private bool isLevitating;
    public LayerMask gridMask;
    private GameObject selectedMagicTarget;
    [HideInInspector] public WalkTarget previousWalkTarget;

    private void Update()
    {

        // TESTE - INPUTS
        if(Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.left * 75f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.right * 75f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 75f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * 75f * Time.deltaTime);
        }



        ray = new Ray(Camera.main.transform.position, Camera.main.transform.rotation * Vector3.forward);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Select a magic target
            if(hit.collider.GetComponent<MagicTarget>() && Input.GetButtonDown("Fire1") && !isLevitating)
            {
                selectedMagicTarget = hit.collider.gameObject;
                selectedMagicTarget.GetComponent<MagicTarget>().StartLevitation(this.transform);
                isLevitating = true;
            }

            // Interact with anything that has tag + interface 'Interactable' by the function 'InteractAction()'
            
            if(hit.collider.CompareTag("Interactable") && Input.GetButtonDown("Fire1") && !isLevitating)
            {
                hit.collider.GetComponent<Interactable>().InteractAction();
            }
            
        }

        if(isLevitating)
        {
            // Move magic target along the grid
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridMask))
            {
                if (hit.collider.CompareTag("MagicGrid"))
                {
                    selectedMagicTarget.transform.position = hit.point;
                }
            }
            else
            {
                //StopLevitation();
            }

            // Stop Levitation Magic
            if(Input.GetButtonUp("Fire1"))
            {
                StopLevitation();
            }
        }

    }

    void StopLevitation()
    {
        isLevitating = false;
        selectedMagicTarget.GetComponent<MagicTarget>().EndLevitation();
        selectedMagicTarget = null;
    }
}
