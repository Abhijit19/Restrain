using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    int enemyHealth = 100;
    bool shieldBool = false;
    bool speedupBool = false;
    bool reFuelBool = false;
    float enemySpeed = 10.0f;
    float shieldTempTimer;
    float speedUpTempTimer;
    float speedUpRefuelTimer;
    //public GameObject speedUpButtonGO;
    //bool timer = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        EnemyDeath();

        if (shieldBool == true)
        {
            ShieldTimer();
        }

        if(speedupBool == true)
        {
            SpeedUp();
        }

        if(speedupBool == false)
        {
            enemySpeed = 10.0f;
            //Debug.Log(enemySpeed);
        }
            SpeedUpRefueling();
        

    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Destroy(other.gameObject);
            if (shieldBool == false)
            { 
                enemyHealth -= 10;
                Debug.Log(this.gameObject.name + " :" + enemyHealth);
            }

            else
            {
                Debug.Log(this.gameObject.name + " :" + enemyHealth);
            }
            
            //Destroy(this.gameObject);
        }

       /* if(other.gameObject.tag == "blaster")
        {
            Debug.Log("Collided with blaster");
            //enemyHealth = 0;
        }*/
    }


    void EnemyDeath()
    {
        if (enemyHealth == 0)
        {
            Destroy(this.gameObject);
        }
    }

    void ShieldTimer()
    {
        
        shieldTempTimer += 1 * Time.deltaTime;
        //Debug.Log(tempTimer);
        if (shieldTempTimer >= 5)
        {
            shieldBool = false;
        }
    }


    void SpeedUp()
    {
        speedUpTempTimer += 1 * Time.deltaTime;
        Debug.Log(speedUpTempTimer);

        if (speedUpTempTimer >= 5)
        {
            speedupBool = false;
            speedUpTempTimer = default;
            reFuelBool = true;

        }
    }

    void SpeedUpRefueling()
    {
        if (reFuelBool == true)
        {
            speedUpRefuelTimer += 1 * Time.deltaTime;
        }

        if(speedUpRefuelTimer >= 5)
        {
            //speedUpButtonGO.SetActive(true);
            reFuelBool = false;
            speedUpRefuelTimer = default;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "HealingPowerUp")
        {
            if (enemyHealth >= 70)
            {
                enemyHealth = 100;
            }
            else
            {
                enemyHealth += 30;
            }
            Debug.Log(this.gameObject.name + " :" + enemyHealth);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "ShieldPowerUp")
        {
            shieldBool = true;
            
            Destroy(collision.gameObject);
             
        }

        if (collision.gameObject.name == "SpeedUpPowerUp")
        {
            speedupBool = true;
            enemySpeed = enemySpeed * 2;
            Debug.Log(enemySpeed);
           /// speedUpButtonGO.SetActive(false);
        }
    }
}
