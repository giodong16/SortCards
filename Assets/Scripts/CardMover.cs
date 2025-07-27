using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    public static CardMover Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public IEnumerator MoveCardGroupParabola(List<Card> group, Column targetCol, float duration = 0.3f, float minHeight = 1f, float bufferHeight = 0.3f)
    {
        if (group == null || group.Count == 0 || targetCol == null) yield break;
        GameManager.Instance.isInteracting = true;
        // Xác định scale mục tiêu
        // Lấy scale toàn cục (world scale)
        Vector3 sourceWorldScale = group[0].transform.lossyScale;
        Vector3 targetWorldScale = targetCol.transform.lossyScale;

        // Tính tỉ lệ scale 
        Vector3 scaleRatio = new Vector3(
            targetWorldScale.x / sourceWorldScale.x,
            targetWorldScale.y / sourceWorldScale.y,
            targetWorldScale.z / sourceWorldScale.z
        );
        AudioManager.Instance.PlaySound(SoundEffectName.ShuffleCard);
        // Tính độ cao lớn nhất cần thiết (max height của mọi cột giữa A và B)
        float height = CalculateParabolaHeight(group[0].transform.position, targetCol.transform.position, minHeight, bufferHeight);

        for (int i = 0; i < group.Count; i++)
        {
            Card card = group[i];
            Column temp = targetCol;

            //float zPos = (targetCol.cards.Count + i+1) * targetCol.spacingZ;
            float zPos = (targetCol.cards.Count + i+1) * targetCol.spacingZ + (targetCol.cards.Count == 0 ? targetCol.firstSpacingZ : 0);

            Vector3 targetPos = temp.transform.TransformPoint(new Vector3(0, 0, zPos));
            Vector3 destScale = targetCol.transform.localScale;

            // Scale chuyển động: animate scale theo t
            StartCoroutine(MoveCardParabolaWithScale(card, targetPos, scaleRatio, duration, height));
            yield return new WaitForSeconds(0.08f); // delay giữa lá
        }

        yield return new WaitForSeconds(duration);
        foreach (var card in group)
        {
            card.transform.SetParent(targetCol.transform, false);
            card.transform.localScale = Vector3.one; // đảm bảo localScale chuẩn sau khi SetParent
        }

        targetCol.PushGroup(group);
        GameManager.Instance.isInteracting = false;
    }


    private float CalculateParabolaHeight(Vector3 start, Vector3 end, float minHeight, float bufferHeight)
    {
        float maxHeight = minHeight;

        Vector3 middle = (start + end) / 2f;

        foreach (Column col in GameManager.Instance.columns)
        {
            Vector3 colPos = col.transform.position;

            bool between = (colPos.x > Mathf.Min(start.x, end.x) && colPos.x < Mathf.Max(start.x, end.x));
            if (between)
            {
                float topZ = col.cards.Count * col.spacingZ;
                float height = col.transform.position.y + topZ;

                if (height > maxHeight)
                {
                    maxHeight = height + bufferHeight;
                }
            }
        }

        return maxHeight;
    }
    private IEnumerator MoveCardParabolaWithScale(Card card, Vector3 targetPos, Vector3 scaleRatio, float duration, float height)
    {
        Vector3 startPos = card.transform.position;
        Vector3 endPos = targetPos;
        Vector3 startScale = card.transform.localScale;
        Vector3 endScale = Vector3.Scale(startScale, scaleRatio);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Parabola y offset
            float parabolaY = Mathf.Sin(t * Mathf.PI) * height;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            currentPos.y += parabolaY;

            // Scale
            card.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            card.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo đúng vị trí cuối
        card.transform.position = endPos;
        card.transform.localScale = endScale;
    }

}
