using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input.x > 0) {
            GetComponent<SpriteRenderer>().flipX = false;
        } else if (input.x < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.Translate(input * 4f * Time.deltaTime);
    }
}
