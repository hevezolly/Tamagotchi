using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PuffBallDesireIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject IconObject;
    [SerializeField]
    private Transform iconsHolder;

    public IconReference AddDesire(Sprite sprite, float alpha = 1f)
    {
        var obj = Instantiate(IconObject, iconsHolder);
        var image = obj.GetComponent<Image>();
        image.sprite = sprite;
        var r = new IconReference(image, () => Destroy(obj));
        r.SetAlpha(alpha);
        return r;
    }
}

public class IconReference
{
    private Image image;
    private Action removeImage;

    public IconReference(Image image, Action removeImage)
    {
        this.image = image;
        this.removeImage = removeImage;
    }
    public void SetAlpha(float alpha)
    {
        var c = new Color(image.color.r, image.color.g, image.color.b);
        c.a = alpha;
        image.color = c;
    }

    public void RemoveImage()
    {
        removeImage?.Invoke();
    }
}
