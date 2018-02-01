using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPoster : MonoBehaviour {

    [SerializeField]
    int framesPerSecond = 10;

    int currentFrame = 0;
    private SpriteRenderer spriteRenderer;
    Sprite[] frames;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
//		spriteRenderer.sprite.
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
