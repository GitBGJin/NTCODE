

#region Usings
using System.Drawing;
using System;
using SmartEP.Utilities.Media.Image.ExtensionMethods;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
#endregion

namespace SmartEP.Utilities.Media.Image
{
    /// <summary>
    /// Used for creating bump maps
    /// </summary>
    public class BumpMap
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public BumpMap()
        {
            Invert = false;
            Direction = Direction.TopBottom;
        }

        #endregion

        #region Protected Properties

        protected virtual Filter EdgeDetectionFilter { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public virtual bool Invert { get; set; }

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public virtual Direction Direction { get; set; }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Sets up the edge detection filter
        /// </summary>
        protected virtual void CreateFilter()
        {
            EdgeDetectionFilter = new Filter(3, 3);
            if (Direction == Direction.TopBottom)
            {
                if (!Invert)
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = 1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 2;
                    EdgeDetectionFilter.MyFilter[2, 0] = 1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[2, 1] = 0;
                    EdgeDetectionFilter.MyFilter[0, 2] = -1;
                    EdgeDetectionFilter.MyFilter[1, 2] = -2;
                    EdgeDetectionFilter.MyFilter[2, 2] = -1;
                }
                else
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = -1;
                    EdgeDetectionFilter.MyFilter[1, 0] = -2;
                    EdgeDetectionFilter.MyFilter[2, 0] = -1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[2, 1] = 0;
                    EdgeDetectionFilter.MyFilter[0, 2] = 1;
                    EdgeDetectionFilter.MyFilter[1, 2] = 2;
                    EdgeDetectionFilter.MyFilter[2, 2] = 1;
                }
            }
            else
            {
                if (!Invert)
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = -1;
                    EdgeDetectionFilter.MyFilter[0, 1] = -2;
                    EdgeDetectionFilter.MyFilter[0, 2] = -1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 2] = 0;
                    EdgeDetectionFilter.MyFilter[2, 0] = 1;
                    EdgeDetectionFilter.MyFilter[2, 1] = 2;
                    EdgeDetectionFilter.MyFilter[2, 2] = 1;
                }
                else
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = 1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 2;
                    EdgeDetectionFilter.MyFilter[0, 2] = 1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 2] = 0;
                    EdgeDetectionFilter.MyFilter[2, 0] = -1;
                    EdgeDetectionFilter.MyFilter[2, 1] = -2;
                    EdgeDetectionFilter.MyFilter[2, 2] = -1;
                }
            }
            EdgeDetectionFilter.Offset = 127;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Creates the bump map
        /// </summary>
        public virtual Bitmap Create(Bitmap Image)
        {
            Image.ThrowIfNull("Image");
            CreateFilter();
            using (Bitmap TempImage = EdgeDetectionFilter.ApplyFilter(Image))
            {
                return TempImage.BlackAndWhite();
            }
        }

        #endregion
    }

    #region Enum

    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        TopBottom = 0,
        LeftRight
    };
    #endregion
}