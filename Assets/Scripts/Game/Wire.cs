﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Wire : MonoBehaviour
{
    [SerializeField]
    private List<Plug> plugs = new List<Plug>();

    [SerializeField]
    private float extraLength = 10.0f;

    private const float SegmentLength = 0.3f;
    private const float LineWidth = 0.1f;

    RaycastHit[] hits = new RaycastHit[1];

    private List<WireSegment> wireSegments = new List<WireSegment>();

    LineRenderer lineRenderer;
    Vector3[] segmentPositions;

    void Awake()
    {
        Assert.IsTrue(plugs.Count == 2);

        Vector3 start = plugs[0].transform.position;
        Vector3 end = plugs[1].transform.position;
        Vector3 direction = end - start;

        int totalSegments = Mathf.CeilToInt((extraLength + direction.magnitude) / SegmentLength);

        float adjustedSegmentLength = direction.magnitude / totalSegments;

        direction = direction.normalized;

        for (int i = 0; i < totalSegments; ++i)
        {
            WireSegment node = (GameObject.Instantiate(Resources.Load("WireSegment") as GameObject)).GetComponent<WireSegment>();
            node.transform.position = start;
            node.previousPosition = start;

            wireSegments.Add(node);

            start += direction * adjustedSegmentLength;
        }

        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;

        segmentPositions = new Vector3[totalSegments];
    }

    void Update()
    {
        DrawLineSegments();
    }

    void FixedUpdate()
    {
        Simulate();

        for (int i = 0; i < 80; ++i)
            AdjustConstraint();
    }


    private void DrawLineSegments()
    {
        for(int i = 0; i < wireSegments.Count; ++ i)
        {
            segmentPositions[i] = new Vector3(wireSegments[i].transform.position.x, wireSegments[i].transform.position.y, 0);
        }

        lineRenderer.positionCount = segmentPositions.Length;
        lineRenderer.SetPositions(segmentPositions);
    }


    private void Simulate()
    {
        foreach(WireSegment segment in wireSegments)
        {
            Vector3 velocity = segment.transform.position - segment.previousPosition;
            segment.previousPosition = segment.transform.position;

            Vector3 position = segment.transform.position + velocity;
            position += (Vector3.down * 0.8f) * Time.fixedDeltaTime;

            segment.transform.position = AdjustSegmentPosition(segment, position);
        }
    }

    private void AdjustConstraint()
    {
        wireSegments[0].transform.position = plugs[0].transform.position;
        wireSegments[wireSegments.Count - 1].transform.position = plugs[1].transform.position;

        for(int i = 0; i < wireSegments.Count - 1; ++i)
        {
            WireSegment segment1 = wireSegments[i];
            WireSegment segment2 = wireSegments[i + 1];

            float distance = (segment1.transform.position - segment2.transform.position).magnitude;
            float difference = Mathf.Abs(distance - SegmentLength);

            Vector2 direction = Vector2.zero;

            if(distance > SegmentLength)
                direction = (segment1.transform.position - segment2.transform.position).normalized;
            else if(distance < SegmentLength)
                direction = (segment2.transform.position - segment1.transform.position).normalized;

            Vector3 movement = direction * difference;
            segment1.transform.position = AdjustSegmentPosition(segment1, segment1.transform.position - (movement * 0.5f));
            segment2.transform.position = AdjustSegmentPosition(segment2, segment2.transform.position + (movement * 0.5f));
        }
    }

    private Vector3 AdjustSegmentPosition(WireSegment segment, Vector3 newPosition)
    {
#if true
        Vector3 direction = segment.transform.position - newPosition;

        Ray ray = new Ray(segment.transform.position, -direction.normalized);
        int result = Physics.RaycastNonAlloc(ray, hits, direction.magnitude);

        for(int i = 0; i < result; ++i)
        {
            RaycastHit hit = hits[i];

            if(hit.collider.GetComponent<Plug>())
                continue;

            if(hit.collider.gameObject.layer == 8)
            {
                newPosition = hit.point + (hit.normal * direction.magnitude);
                break;
            }
        }
#endif

        return new Vector3(newPosition.x, newPosition.y, 0);
    }
}