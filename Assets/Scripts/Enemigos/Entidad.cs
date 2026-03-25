using UnityEngine;

public abstract class Entidad : MonoBehaviour
{
    private int vidaMaxima = 100;
    protected int vida;
    private int dano = 30;

    public virtual int VidaMaxima
    {
        get { return vidaMaxima; }
        protected set { vidaMaxima = value; }
    }

    public virtual int Dano
    {
        get { return dano; }
        protected set { dano = value; }
    }

    protected virtual void Start()
    {
        vida = VidaMaxima; 
    }

    public virtual void recibirDano(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        Destroy(gameObject);
    }
}
