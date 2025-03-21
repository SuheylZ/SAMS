using System;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ImageComboItem : object
    {
        // forecolor: transparent = inherit
        private Color forecolor = Color.FromKnownColor(
                KnownColor.Transparent);
        private bool mark = false;
        private int imageindex = -1;
        private object tag = null;
        private string text = null;

        // constructors
        public ImageComboItem()
        {
        }

        public ImageComboItem(string Text)
        {
            text = Text;
        }

        public ImageComboItem(string Text, int ImageIndex)
        {
            text = Text;
            imageindex = ImageIndex;
        }

        public ImageComboItem(string Text, int ImageIndex, bool Mark)
        {
            text = Text;
            imageindex = ImageIndex;
            mark = Mark;
        }

        public ImageComboItem(string Text, int ImageIndex,
            bool Mark, Color ForeColor)
        {
            text = Text;
            imageindex = ImageIndex;
            mark = Mark;
            forecolor = ForeColor;
        }

        public ImageComboItem(string Text, int ImageIndex,
               bool Mark, Color ForeColor, object Tag)
        {
            text = Text;
            imageindex = ImageIndex;
            mark = Mark;
            forecolor = ForeColor;
            tag = Tag;
        }

        public ImageComboItem(string Text, int ImageIndex, object Tag)
        {
            text = Text;
            imageindex = ImageIndex;
            tag = Tag;
        }

        // forecolor
        public Color ForeColor
        {
            get
            {
                return forecolor;
            }
            set
            {
                forecolor = value;
            }
        }

        // image index
        public int ImageIndex
        {
            get
            {
                return imageindex;
            }
            set
            {
                imageindex = value;
            }
        }

        // mark (bold)
        public bool Mark
        {
            get
            {
                return mark;
            }
            set
            {
                mark = value;
            }
        }

        // tag
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        // item text
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        // ToString() should return item text
        public override string ToString()
        {
            return text;
        }

    }

    public class ImageCombo : ComboBox
    {
        private ImageList imgs = new ImageList();

        private int _ImageGap = 7;
        // constructor
        public ImageCombo()
        {
            // set draw mode to owner draw
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        // ImageList property
        public ImageList ImageList
        {
            get
            {
                return imgs;
            }
            set
            {
                imgs = value;
            }
        }

        // customized drawing process
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // draw background & focus rect
            e.DrawBackground();
            e.DrawFocusRectangle();

            // check if it is an item from the Items collection
            if (e.Index < 0)

                // not an item, draw the text (indented)
                e.Graphics.DrawString(this.Text, e.Font,
                        new SolidBrush(e.ForeColor), e.Bounds.Left +
                        imgs.ImageSize.Width + _ImageGap, e.Bounds.Top);

            else
            {

                // check if item is an ImageComboItem
                if (this.Items[e.Index].GetType() == typeof(ImageComboItem))
                {

                    // get item to draw
                    ImageComboItem item = (ImageComboItem)
                        this.Items[e.Index];

                    // get forecolor & font
                    Color forecolor = (item.ForeColor !=
                         Color.FromKnownColor(KnownColor.Transparent)) ?
                         item.ForeColor : e.ForeColor;
                    Font font = item.Mark ? new Font(e.Font,
                         FontStyle.Bold) : e.Font;

                    // -1: no image
                    if (item.ImageIndex != -1)
                    {
                        // draw image, then draw text next to it
                        this.ImageList.Draw(e.Graphics,
                           e.Bounds.Left, e.Bounds.Top, item.ImageIndex);
                        e.Graphics.DrawString(item.Text, font,
                           new SolidBrush(forecolor), e.Bounds.Left +
                           imgs.ImageSize.Width + _ImageGap, e.Bounds.Top);
                    }
                    else
                        // draw text (indented)
                        e.Graphics.DrawString(item.Text, font,
                            new SolidBrush(forecolor), e.Bounds.Left +
                            imgs.ImageSize.Width + _ImageGap, e.Bounds.Top);

                }
                else

                    // it is not an ImageComboItem, draw it
                    e.Graphics.DrawString(this.Items[e.Index].ToString(),
                      e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left +
                      imgs.ImageSize.Width + _ImageGap, e.Bounds.Top);

            }

            base.OnDrawItem(e);
        }

    }
}