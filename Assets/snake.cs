using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();

    public Transform segmentPrefab;

    public int intialSize = 4;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position += new Vector3(_direction.x, _direction.y, 0.0f);
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        // Destroy all snake segments except the head
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        // Clear and reinitialize the segments list
        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < this.intialSize; i++)
        {
            Transform segment = Instantiate(this.segmentPrefab);
            segment.position = new Vector3(
                this.transform.position.x - i,
                this.transform.position.y,
                0.0f
            );
            _segments.Add(segment);
        }

        // Reset the snake's position
        this.transform.position = Vector3.zero;
        _direction = Vector2.right; // Reset direction to the initial state
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }
        else if (other.tag == "Obstacle" || IsCollidingWithSelf(other))
        {
            ResetState();
        }
    }

    private bool IsCollidingWithSelf(Collider2D other)
    {
        // Check if the collider belongs to one of the snake's body segments
        for (int i = 1; i < _segments.Count; i++) // Start from 1 to skip the head
        {
            if (other.transform == _segments[i])
            {
                return true;
            }
        }
        return false;
    }
}
