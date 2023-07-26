using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class TrailCollision : MonoBehaviour
{
    public delegate void OnPieceCut(GameObject collision);
    public static event OnPieceCut onPieceCut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == DESTROYER_GAMEOBJECT.Piece.ToString())
            onPieceCut?.Invoke(collision.gameObject);
    }
}
