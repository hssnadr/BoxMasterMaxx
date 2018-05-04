using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour {
    public uint playerIndex;

    float time;

    public float timeUntilMove = 3.0f;

    void Start()
    {
        time = Time.time;
    }

    void Update()
    {
        if (GameManager.instance.gameHasStarted)
        {
            if (time + timeUntilMove <= Time.time)
            {
                time = Time.time;
                this.GetComponent<RectTransform>().position = new Vector2(Random.Range(0, 1920), Random.Range(0, 1080));
            }
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Input.mousePosition;
                var rect = this.GetComponent<RectTransform>().rect;
                rect.position = this.GetComponent<RectTransform>().position;
                Debug.Log("mousePos: " + mousePos);
                Debug.Log("rect: " + rect);

                if (rect.Contains(mousePos))
                {
                    GameManager.instance.ScoreUp(playerIndex);
                    Debug.Log("worked");
                    time = Time.time;
                    this.GetComponent<RectTransform>().position = new Vector2(Random.Range(0, 1920), Random.Range(0, 1080));
                }
            }
        }
    }
}
