using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private enum DirectionType
        {
            None = -1,
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3
        }

#pragma warning disable 649
        [SerializeField] private RectTransform handlerContainerRectTransform;
        [SerializeField] private RectTransform handlerRectTransform;
        [SerializeField, Range(50f, 150f)] private float range;
        [SerializeField] private float magnitudeMultiplier = 1f;
        [SerializeField] private UnityEvent<Vector2> outputUnityEvent;
        [SerializeField] private List<Direction> directionList;
#pragma warning restore 649

        #region Constants
        
        #endregion

        private void Start()
        {
            Init();
        }
        
        private void Init()
        {
            UpdateDirection(Vector2.zero);
            UpdateHandleRectPosition(Vector2.zero);
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(handlerContainerRectTransform, eventData.position,
                eventData.pressEventCamera, out Vector2 position);
            position = ApplySizeDelta(position);
            Vector2 clampPosition = ClampPosition(position);
            UpdateDirection(clampPosition);
            UpdateOutputEventValue(clampPosition * magnitudeMultiplier);
            UpdateHandleRectPosition(clampPosition * range);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UpdateOutputEventValue(Vector2.zero);
            UpdateDirection(Vector2.zero);
            UpdateHandleRectPosition(Vector2.zero);
        }
        
        private void UpdateHandleRectPosition(Vector2 rectPosition)
        {
            handlerRectTransform.anchoredPosition = rectPosition;
        }

        private Vector2 ApplySizeDelta(Vector2 position)
        {
            Vector2 containerSizeDelta = handlerContainerRectTransform.sizeDelta;
            float x = position.x / containerSizeDelta.x * 2.5f;
            float y = position.y / containerSizeDelta.y * 2.5f;
            return new Vector2(x, y);
        }

        private Vector2 ClampPosition(Vector2 position)
        {
            return Vector2.ClampMagnitude(position, 1);
        }

        private void UpdateOutputEventValue(Vector2 value)
        {
            outputUnityEvent?.Invoke(value);
        }

        private void UpdateDirection(Vector2 position)
        {
            DirectionType directionType = Direction.CheckDirection((position.x, position.y));
            foreach (var direction in directionList)
            {
                direction.DirectionImage.enabled = direction.DirectionType == directionType;
            }
        }

        [Serializable]
        private class Direction
        {
#pragma warning disable 649
            [SerializeField] private DirectionType directionType;
            [SerializeField] private Image directionImage;
#pragma warning restore 649

            #region Constants
            private static readonly List<((float xMin, float xMax) xRange, (float yMin, float yMax) yRange)> RangeList =
                new List<((float xMin, float xMax), (float yMin, float yMax))>
                {
                    ((-1, 0), (0, 1)), // TopLeft
                    ((0, 1), (0, 1)), // TopRight
                    ((-1, 0), (-1, 0)), // BottomLeft
                    ((0, 1), (-1, 0)) // BottomRight
                };
            #endregion

            public DirectionType DirectionType => directionType;
            public Image DirectionImage => directionImage;

            public static DirectionType CheckDirection((float x, float y) position)
            {
                for (int i = 0; i < RangeList.Count; i++)
                {
                    if (RangeList[i].xRange.xMin < position.x && position.x < RangeList[i].xRange.xMax)
                    {
                        if (RangeList[i].yRange.yMin < position.y && position.y < RangeList[i].yRange.yMax)
                        {
                            return (DirectionType) i;
                        }
                    }
                }
                return DirectionType.None;
            }
        }
    }
}