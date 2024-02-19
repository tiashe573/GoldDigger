using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hook : MonoBehaviour
{
    // Start is called before the first frame update
    public float num = 0.25f;
    // void Start()
    // {
    //     Debug.Log(num);
    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
    public float rotateSpeed;
    public int rotateDir;
    public float moveSpeed;
    private bool isMoving;
    private float moveTimer;
    private bool returnToInitPos;
    private Vector3 originalPosition;
    public SpriteRenderer sr;
    public Transform hookHeadTrans;
    public Gold gold;
    public int money;
    public Text moneyText;
    private void Start()
    {
        originalPosition = transform.position;
        moneyText.text = money.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
        }
        if (isMoving)
        {
            if (returnToInitPos)
            {
                HookReturn();
            }
            else
            {
                HookMove();
            }
        }
        else
        {
            HookRotate();
        }
    }

    private void HookRotate()
    {
        float angle = (transform.eulerAngles.z + 180) % 360 - 180;
        if (angle >= 60)
        {
            rotateDir = -1;
        }
        else if (angle <= -60)
        {
            rotateDir = 1;
        }
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime * rotateDir);
    }

    private void HookMove()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        moveTimer += Time.deltaTime;
        if (moveTimer >= 2)
        {
            returnToInitPos = true;
        }
        sr.size = new Vector2(sr.size.x, sr.size.y - moveSpeed * Time.deltaTime);
    }

    private void HookReturn()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
        sr.size = new Vector2(sr.size.x, sr.size.y + moveSpeed * Time.deltaTime);
        if (transform.position.y >= originalPosition.y)
        {
            isMoving = false;
            returnToInitPos = false;
            moveTimer = 0;
            moveSpeed = 5;
            if (gold != null)
            {
                money += gold.price;
                moneyText.text = money.ToString();
                Destroy(gold.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gold"))
        {
            gold = collision.GetComponent<Gold>();
            money += gold.price;
            collision.transform.SetParent(hookHeadTrans);
            collision.transform.localEulerAngles = Vector3.zero;
            collision.transform.localPosition = Vector3.zero;
            moveSpeed = gold.speed;
            returnToInitPos = true;
        }
    }
}


