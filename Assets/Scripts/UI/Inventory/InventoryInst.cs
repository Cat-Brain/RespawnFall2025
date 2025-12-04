using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventoryInst : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public InventoryController controller;
    public RectTransform rectTransform;
    public Image image;

    public InventoryItem item;

    [HideInInspector] public Vector2 desiredPos;
    [HideInInspector] public Vector2Int gridPos = -Vector2Int.one;
    [HideInInspector] public int index = -1;
    [HideInInspector] public Vector2 vel = Vector2.zero;
    [HideInInspector] public bool dragging = false;

    [ProButton]
    public void Init()
    {
        Clean();
    }

    void Update()
    {
        if (!Application.isPlaying && image && item && item.sprite)
            image.sprite = item.sprite;
    }

    void LateUpdate()
    {
        if (!Application.isPlaying)
            return;

        if (dragging && controller.manager.gameState != GameState.INVENTORY)
            EndDrag();

        Vector2 pos = rectTransform.anchoredPosition;
        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, controller.itemSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.y, ref vel.y, desiredPos.y, controller.itemSpring);
        rectTransform.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (controller.manager.gameState != GameState.INVENTORY)
            return;
        controller.RemoveFromInventory(this);
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
            desiredPos += controller.GlobalToLocalOffset(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragging)
            EndDrag();
    }

    public void EndDrag()
    {
        CleanDimensions();
        dragging = false;
        controller.AddToInventory(this);
    }

    public void CleanPivot()
    {
        rectTransform.pivot = Vector2.one / (2 * item.dimensions);
    }

    public void CleanDimensions()
    {
        rectTransform.localScale = Vector3.one * Mathf.Max(item.dimensions.x, item.dimensions.y);
        rectTransform.sizeDelta = controller.cellWidth * Vector2.one;
    }

    public void Clean()
    {
        image.sprite = item.sprite;

        CleanPivot();
        CleanDimensions();
        
        if (gridPos == -Vector2Int.one)
        {
            index = controller.bufferItems.FindIndex((item) => item == this);
            controller.SnapBufferPosition(this);
        }
        else
        {
            index = controller.items.FindIndex((item) => item == this);
            controller.SnapInventoryPosition(this);
        }

        rectTransform.anchoredPosition = desiredPos;
    }
}