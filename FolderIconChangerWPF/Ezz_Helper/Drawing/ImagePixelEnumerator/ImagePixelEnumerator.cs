using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

//'' Image Per Pixel Enumeration, Pixel Format Conversion and More V2 '' By Smart K8
//''http://www.codeproject.com/Articles/66550/Image-Per-Pixel-Enumeration-Pixel-Format-Conversio
//''Source code has been copied to one file (as-is) - just added SimpleHelper class.
namespace Ezz_Helper.Drawing.ImagePixelEnumerator
{

    #region Extensions

    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {
        #region PixelFormat

        /// <summary>
        /// Gets the bit count for a given pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The bit count.</returns>
        public static Byte GetBitDepth(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return 1;

                case PixelFormat.Format4bppIndexed:
                    return 4;

                case PixelFormat.Format8bppIndexed:
                    return 8;

                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;

                case PixelFormat.Format24bppRgb:
                    return 24;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;

                case PixelFormat.Format48bppRgb:
                    return 48;

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;

                default:
                    String message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the available color count for a given pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The available color count.</returns>
        public static UInt16 GetColorCount(this PixelFormat pixelFormat)
        {
            // checks whether a pixel format is indexed, otherwise throw an exception
            if (!pixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot retrieve color count for a non-indexed format '{0}'.", pixelFormat);
                throw new NotSupportedException(message);
            }

            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return 2;

                case PixelFormat.Format4bppIndexed:
                    return 16;

                case PixelFormat.Format8bppIndexed:
                    return 256;

                default:
                    String message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the friendly name of the pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns></returns>
        public static String GetFriendlyName(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return "Indexed (2 colors)";

                case PixelFormat.Format4bppIndexed:
                    return "Indexed (16 colors)";

                case PixelFormat.Format8bppIndexed:
                    return "Indexed (256 colors)";

                case PixelFormat.Format16bppGrayScale:
                    return "Grayscale (65536 shades)";

                case PixelFormat.Format16bppArgb1555:
                    return "Highcolor + Alpha mask (32768 colors)";

                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return "Highcolor (65536 colors)";

                case PixelFormat.Format24bppRgb:
                    return "Truecolor (24-bit)";

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return "Truecolor + Alpha (32-bit)";

                case PixelFormat.Format32bppRgb:
                    return "Truecolor (32-bit)";

                case PixelFormat.Format48bppRgb:
                    return "Truecolor (48-bit)";

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return "Truecolor + Alpha (64-bit)";

                default:
                    String message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Determines whether the specified pixel format is indexed.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is indexed; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsIndexed(this PixelFormat pixelFormat)
        {
            return (pixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed;
        }

        /// <summary>
        /// Determines whether the specified pixel format is supported.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is supported; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsSupported(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format64bppArgb:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the format by color count.
        /// </summary>
        public static PixelFormat GetFormatByColorCount(Int32 colorCount)
        {
            if (colorCount <= 0 || colorCount > 256)
            {
                String message = string.Format("A color count '{0}' not supported!", colorCount);
                throw new NotSupportedException(message);
            }

            PixelFormat result = PixelFormat.Format1bppIndexed;

            if (colorCount > 16)
            {
                result = PixelFormat.Format8bppIndexed;
            }
            else if (colorCount > 2)
            {
                result = PixelFormat.Format4bppIndexed;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified pixel format has an alpha channel.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format has an alpha channel; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasAlpha(this PixelFormat pixelFormat)
        {
            return (pixelFormat & PixelFormat.Alpha) == PixelFormat.Alpha ||
                   (pixelFormat & PixelFormat.PAlpha) == PixelFormat.PAlpha;
        }

        /// <summary>
        /// Determines whether [is deep color] [the specified pixel format].
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if [is deep color] [the specified pixel format]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsDeepColor(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region Image
        #region | Palette methods |

        /// <summary>
        /// Gets the palette color count.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static Int32 GetPaletteColorCount(this Image image)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has an indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot retrieve a color count from a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // returns the color count
            return image.Palette.Entries.Length;
        }

        /// <summary>
        /// Gets the palette of an indexed image.
        /// </summary>
        /// <param name="image">The source image.</param>
        public static List<Color> GetPalette(this Image image)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has an indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot retrieve a palette from a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // retrieves and returns the palette
            return image.Palette.Entries.ToList();
        }

        /// <summary>
        /// Sets the palette of an indexed image.
        /// </summary>
        /// <param name="image">The target image.</param>
        /// <param name="palette">The palette.</param>
        public static void SetPalette(this Image image, List<Color> palette)
        {
            // checks whether a palette is valid
            if (palette == null)
            {
                const String message = "Cannot assign a null palette.";
                throw new ArgumentNullException(message);
            }

            // checks whether a target image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot store a palette to a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // retrieves a target image palette
            ColorPalette imagePalette = image.Palette;

            // checks if the palette can fit into the image palette
            if (palette.Count > imagePalette.Entries.Length)
            {
                String message = string.Format("Cannot store a palette with '{0}' colors intto an image palette where only '{1}' colors are allowed.", palette.Count, imagePalette.Entries.Length);
                throw new ArgumentOutOfRangeException(message);
            }

            // copies all color entries
            for (Int32 index = 0; index < palette.Count; index++)
            {
                imagePalette.Entries[index] = palette[index];
            }

            // assigns the palette to the target image
            image.Palette = imagePalette;
        }

        #endregion
        #endregion

        #region IEnumerable
        /// <summary>
        /// Selects distinct items by a given selector.
        /// </summary>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> selector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();
            return items.Where(item => keys.Add(selector(item)));
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<T> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence was empty");

                T max = sourceIterator.Current;
                TKey maxKey = selector.Invoke(max);

                while (sourceIterator.MoveNext())
                {
                    T candidate = sourceIterator.Current;
                    TKey candidateProjected = selector.Invoke(candidate);

                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }
        #endregion

    }//Extend

    #endregion //Extensions

    #region ColorCaches

    public enum ColorModel
    {
        /// <summary>
        /// The RGB color model is an additive color model in which red, green, and blue light is added together 
        /// in various ways to reproduce a broad array of colors. The name of the model comes from the initials 
        /// of the three additive primary colors, red, green, and blue.
        /// </summary>
        RedGreenBlue = 0,

        /// <summary>
        /// HSL is a common cylindrical-coordinate representations of points in an RGB color model, which rearrange 
        /// the geometry of RGB in an attempt to be more perceptually relevant than the cartesian representation.
        /// </summary>
        HueSaturationBrightness = 1,
        HueSaturationLuminance = 1,

        /// <summary>
        /// A Lab color space is a color-opponent space with dimension L for lightness and a and b for the 
        /// color-opponent dimensions, based on nonlinearly compressed CIE XYZ color space coordinates.
        /// </summary>
        LabColorSpace = 2,

        /// <summary>
        /// XYZ color space
        /// </summary>
        XYZ = 3,
    }
    public interface IColorCache
    {
        /// <summary>
        /// Prepares color cache for next use.
        /// </summary>
        void Prepare();

        /// <summary>
        /// Called when a palette is about to be cached, or precached.
        /// </summary>
        /// <param name="palette">The palette.</param>
        void CachePalette(IList<Color> palette);

        /// <summary>
        /// Called when palette index is about to be retrieve for a given color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        void GetColorPaletteIndex(Color color, out Int32 paletteIndex);
    }
    public abstract class BaseColorCache : IColorCache
    {
        #region | Fields |

        private readonly ConcurrentDictionary<Int32, Int32> cache;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets or sets the color model.
        /// </summary>
        /// <value>The color model.</value>
        protected ColorModel ColorModel { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is color model supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is color model supported; otherwise, <c>false</c>.
        /// </value>
        public abstract Boolean IsColorModelSupported { get; }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseColorCache"/> class.
        /// </summary>
        protected BaseColorCache()
        {
            cache = new ConcurrentDictionary<Int32, Int32>();
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Changes the color model.
        /// </summary>
        /// <param name="colorModel_1">The color model.</param>
        public void ChangeColorModel(ColorModel colorModel_1)
        {
            ColorModel = colorModel_1;
        }

        #endregion

        #region << Abstract methods |

        /// <summary>
        /// Called when a palette is about to be cached, or precached.
        /// </summary>
        /// <param name="palette">The palette.</param>
        protected abstract void OnCachePalette(IList<Color> palette);

        /// <summary>
        /// Called when palette index is about to be retrieve for a given color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        protected abstract void OnGetColorPaletteIndex(Color color, out Int32 paletteIndex);

        #endregion

        #region << IColorCache >>

        /// <summary>
        /// See <see cref="IColorCache.Prepare"/> for more details.
        /// </summary>
        public virtual void Prepare()
        {
            cache.Clear();
        }

        /// <summary>
        /// See <see cref="IColorCache.CachePalette"/> for more details.
        /// </summary>
        public void CachePalette(IList<Color> palette)
        {
            OnCachePalette(palette);
        }

        /// <summary>
        /// See <see cref="IColorCache.GetColorPaletteIndex"/> for more details.
        /// </summary>
        public void GetColorPaletteIndex(Color color, out Int32 paletteIndex)
        {
            Int32 key = color.R << 16 | color.G << 8 | color.B;

            paletteIndex = cache.AddOrUpdate(key,
                colorKey =>
                {
                    Int32 paletteIndexInside;
                    OnGetColorPaletteIndex(color, out paletteIndexInside);
                    return paletteIndexInside;
                },
                (colorKey, inputIndex) => inputIndex);
        }

        #endregion
    }
    //
    public class EuclideanDistanceColorCache : BaseColorCache
    {
        #region | Fields |

        private IList<Color> palette;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets a value indicating whether this instance is color model supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is color model supported; otherwise, <c>false</c>.
        /// </value>
        public override Boolean IsColorModelSupported
        {
            get { return true; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="EuclideanDistanceColorCache"/> class.
        /// </summary>
        public EuclideanDistanceColorCache()
        {
            ColorModel = ColorModel.RedGreenBlue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EuclideanDistanceColorCache"/> class.
        /// </summary>
        /// <param name="colorModel">The color model.</param>
        public EuclideanDistanceColorCache(ColorModel colorModel)
        {
            ColorModel = colorModel;
        }

        #endregion

        #region << BaseColorCache >>

        /// <summary>
        /// See <see cref="BaseColorCache.OnCachePalette"/> for more details.
        /// </summary>
        protected override void OnCachePalette(IList<Color> palette)
        {
            this.palette = palette;
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnGetColorPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetColorPaletteIndex(Color color, out Int32 paletteIndex)
        {
            paletteIndex = ColorModelHelper.GetEuclideanDistance(color, ColorModel, palette);
        }

        #endregion
    }
    public class OctreeCacheNode
    {
        private static readonly Byte[] Mask = new Byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

        private readonly OctreeCacheNode[] nodes;
        private readonly Dictionary<Int32, Color> entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeCacheNode"/> class.
        /// </summary>
        public OctreeCacheNode()
        {
            nodes = new OctreeCacheNode[8];
            entries = new Dictionary<Int32, Color>();
        }

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="level">The level.</param>
        public void AddColor(Color color, Int32 paletteIndex, Int32 level)
        {
            // if this node is a leaf, then increase a color amount, and pixel presence
            entries.Add(paletteIndex, color);

            if (level < 8) // otherwise goes one level deeper
            {
                // calculates an index for the next sub-branch
                Int32 index = GetColorIndexAtLevel(color, level);

                // if that branch doesn't exist, grows it
                if (nodes[index] == null)
                {
                    nodes[index] = new OctreeCacheNode();
                }

                // adds a color to that branch
                nodes[index].AddColor(color, paletteIndex, level + 1);
            }
        }

        /// <summary>
        /// Gets the index of the palette.
        /// </summary>
        public Dictionary<Int32, Color> GetPaletteIndex(Color color, Int32 level)
        {
            Dictionary<Int32, Color> result = entries;

            if (level < 8)
            {
                Int32 index = GetColorIndexAtLevel(color, level);

                if (nodes[index] != null)
                {
                    result = nodes[index].GetPaletteIndex(color, level + 1);
                }
            }

            return result;
        }

        private static Int32 GetColorIndexAtLevel(Color color, Int32 level)
        {
            return ((color.R & Mask[level]) == Mask[level] ? 4 : 0) |
                   ((color.G & Mask[level]) == Mask[level] ? 2 : 0) |
                   ((color.B & Mask[level]) == Mask[level] ? 1 : 0);
        }
    }
    public class OctreeColorCache : BaseColorCache
    {
        #region | Fields |

        private OctreeCacheNode root;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets a value indicating whether this instance is color model supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is color model supported; otherwise, <c>false</c>.
        /// </value>
        public override Boolean IsColorModelSupported
        {
            get { return false; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeColorCache"/> class.
        /// </summary>
        public OctreeColorCache()
        {
            ColorModel = ColorModel.RedGreenBlue;
            root = new OctreeCacheNode();
        }

        #endregion

        #region << BaseColorCache >>

        /// <summary>
        /// See <see cref="BaseColorCache.Prepare"/> for more details.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();
            root = new OctreeCacheNode();
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnCachePalette"/> for more details.
        /// </summary>
        protected override void OnCachePalette(IList<Color> palette)
        {
            Int32 index = 0;

            foreach (Color color in palette)
            {
                root.AddColor(color, index++, 0);
            }
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnGetColorPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetColorPaletteIndex(Color color, out Int32 paletteIndex)
        {
            Dictionary<Int32, Color> candidates = root.GetPaletteIndex(color, 0);

            paletteIndex = 0;
            Int32 index = 0;
            Int32 colorIndex = ColorModelHelper.GetEuclideanDistance(color, ColorModel, candidates.Values.ToList());

            foreach (Int32 colorPaletteIndex in candidates.Keys)
            {
                if (index == colorIndex)
                {
                    paletteIndex = colorPaletteIndex;
                    break;
                }

                index++;
            }
        }

        #endregion
    }

    #endregion //ColorCaches

    #region Ditherers
    public interface IColorDitherer : IPathProvider
    {
        /// <summary>
        /// Gets a value indicating whether this ditherer uses only actually process pixel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this ditherer is inplace; otherwise, <c>false</c>.
        /// </value>
        Boolean IsInplace { get; }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        void Prepare(IColorQuantizer quantizer, Int32 colorCount, ImageBuffer sourceBuffer, ImageBuffer targetBuffer);

        /// <summary>
        /// Processes the specified buffer.
        /// </summary>
        Boolean ProcessPixel(Int32 passIndex, Pixel sourcePixel, Pixel targetPixel);

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        void Finish();
    }
    #endregion

    #region PathProviders

    public interface IPathProvider
    {
        /// <summary>
        /// Retrieves the path throughout the image to determine the order in which pixels will be scanned.
        /// </summary>
        IList<Point> GetPointPath(Int32 width, Int32 height);
    }
    public class StandardPathProvider : IPathProvider
    {
        public IList<Point> GetPointPath(Int32 width, Int32 height)
        {
            List<Point> result = new List<Point>(width * height);

            for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    Point point = new Point(x, y);
                    result.Add(point);
                }

            return result;
        }

        public static IList<Point> CreatePath(Int32 width, Int32 height)
        {
            StandardPathProvider result = new StandardPathProvider();
            return result.GetPointPath(width, height);
        }
    }

    #endregion

    #region Quantizers

    /// <summary>
    /// This interface provides a color quantization capabilities.
    /// </summary>
    public interface IColorQuantizer : IPathProvider
    {
        /// <summary>
        /// Gets a value indicating whether to allow parallel processing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to allow parallel processing; otherwise, <c>false</c>.
        /// </value>
        Boolean AllowParallel { get; }

        /// <summary>
        /// Prepares the quantizer for image processing.
        /// </summary>
        void Prepare(Image image);

        /// <summary>
        /// Adds the color to quantizer.
        /// </summary>
        void AddColor(Color color, Int32 x, Int32 y);

        /// <summary>
        /// Gets the palette with specified count of the colors.
        /// </summary>
        List<Color> GetPalette(Int32 colorCount);

        /// <summary>
        /// Gets the index of the palette for specific color.
        /// </summary>
        Int32 GetPaletteIndex(Color color, Int32 x, Int32 y);

        /// <summary>
        /// Gets the color count.
        /// </summary>
        Int32 GetColorCount();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Finish();
    }
    public abstract class BaseColorQuantizer : IColorQuantizer
    {
        #region | Constants |

        /// <summary>
        /// This index will represent invalid palette index.
        /// </summary>
        protected const Int32 InvalidIndex = -1;

        #endregion

        #region | Fields |

        private Boolean paletteFound;
        private Int64 uniqueColorIndex;
        private IPathProvider pathProvider;
        protected readonly ConcurrentDictionary<Int32, Int16> UniqueColors;

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseColorQuantizer"/> class.
        /// </summary>
        protected BaseColorQuantizer()
        {
            pathProvider = null;
            uniqueColorIndex = -1;
            UniqueColors = new ConcurrentDictionary<Int32, Int16>();
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Changes the path provider.
        /// </summary>
        /// <param name="pathProvider">The path provider.</param>
        public void ChangePathProvider(IPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        #endregion

        #region | Helper methods |

        private IPathProvider GetPathProvider()
        {
            // if there is no path provider, it attempts to create a default one; integrated in the quantizer
            IPathProvider result = pathProvider ?? (pathProvider = OnCreateDefaultPathProvider());

            // if the provider exists; or default one was created for these purposes.. use it
            if (result == null)
            {
                String message = string.Format("The path provider is not initialized! Please use SetPathProvider() method on quantizer.");
                throw new ArgumentNullException(message);
            }

            // provider was obtained somehow, use it
            return result;
        }

        #endregion

        #region | Abstract/virtual methods |

        /// <summary>
        /// Called when quantizer is about to be prepared for next round.
        /// </summary>
        protected virtual void OnPrepare(Image image)
        {
            uniqueColorIndex = -1;
            paletteFound = false;
            UniqueColors.Clear();
        }

        /// <summary>
        /// Called when color is to be added.
        /// </summary>
        protected virtual void OnAddColor(Color color, Int32 key, Int32 x, Int32 y)
        {
            UniqueColors.AddOrUpdate(key,
                colorKey => (Byte)Interlocked.Increment(ref uniqueColorIndex),
                (colorKey, colorIndex) => colorIndex);
        }

        /// <summary>
        /// Called when a need to create default path provider arisen.
        /// </summary>
        protected virtual IPathProvider OnCreateDefaultPathProvider()
        {
            pathProvider = new StandardPathProvider();
            return new StandardPathProvider();
        }

        /// <summary>
        /// Called when quantized palette is needed.
        /// </summary>
        protected virtual List<Color> OnGetPalette(Int32 colorCount)
        {
            // early optimalization, in case the color count is lower than total unique color count
            if (UniqueColors.Count > 0 && colorCount >= UniqueColors.Count)
            {
                // palette was found
                paletteFound = true;

                // generates the palette from unique numbers
                return UniqueColors.
                    OrderBy(pair => pair.Value).
                    Select(pair => Color.FromArgb(pair.Key)).
                    Select(color => Color.FromArgb(255, color.R, color.G, color.B)).
                    ToList();
            }

            // otherwise make it descendant responsibility
            return null;
        }

        /// <summary>
        /// Called when get palette index for a given color should be returned.
        /// </summary>
        protected virtual void OnGetPaletteIndex(Color color, Int32 key, Int32 x, Int32 y, out Int32 paletteIndex)
        {
            // by default unknown index is returned
            paletteIndex = InvalidIndex;
            Int16 foundIndex;

            // if we previously found palette quickly (without quantization), use it
            if (paletteFound && UniqueColors.TryGetValue(key, out foundIndex))
            {
                paletteIndex = foundIndex;
            }
        }

        /// <summary>
        /// Called when get color count.
        /// </summary>
        protected virtual Int32 OnGetColorCount()
        {
            return UniqueColors.Count;
        }

        /// <summary>
        /// Called when about to clear left-overs after quantization.
        /// </summary>
        protected virtual void OnFinish()
        {
            // do nothing here
        }

        #endregion

        #region << IPathProvider >>

        /// <summary>
        /// See <see cref="IPathProvider.GetPointPath"/> for more details.
        /// </summary>
        public IList<Point> GetPointPath(Int32 width, Int32 heigth)
        {
            return GetPathProvider().GetPointPath(width, heigth);
        }

        #endregion

        #region << IColorQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public abstract Boolean AllowParallel { get; }

        /// <summary>
        /// See <see cref="IColorQuantizer.Prepare"/> for more details.
        /// </summary>
        public void Prepare(Image image)
        {
            OnPrepare(image);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.AddColor"/> for more details.
        /// </summary>
        public void AddColor(Color color, Int32 x, Int32 y)
        {
            Int32 key;
            color = QuantizationHelper.ConvertAlpha(color, out key);
            OnAddColor(color, key, x, y);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetColorCount"/> for more details.
        /// </summary>
        public Int32 GetColorCount()
        {
            return OnGetColorCount();
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetPalette"/> for more details.
        /// </summary>
        public List<Color> GetPalette(Int32 colorCount)
        {
            return OnGetPalette(colorCount);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetPaletteIndex"/> for more details.
        /// </summary>
        public Int32 GetPaletteIndex(Color color, Int32 x, Int32 y)
        {
            Int32 result, key;
            color = QuantizationHelper.ConvertAlpha(color, out key);
            OnGetPaletteIndex(color, key, x, y, out result);
            return result;
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.Finish"/> for more details.
        /// </summary>
        public void Finish()
        {
            OnFinish();
        }

        #endregion
    }
    public abstract class BaseColorCacheQuantizer : BaseColorQuantizer
    {
        #region | Fields |

        private IColorCache colorCache;

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseColorCacheQuantizer"/> class.
        /// </summary>
        protected BaseColorCacheQuantizer()
        {
            colorCache = null;
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Changes the cache provider.
        /// </summary>
        /// <param name="colorCache">The color cache.</param>
        public void ChangeCacheProvider(IColorCache colorCache)
        {
            this.colorCache = colorCache;
        }

        /// <summary>
        /// Caches the palette.
        /// </summary>
        /// <param name="palette">The palette.</param>
        public void CachePalette(IList<Color> palette)
        {
            GetColorCache().CachePalette(palette);
        }

        #endregion

        #region | Helper methods |

        private IColorCache GetColorCache()
        {
            // if there is no cache, it attempts to create a default cache; integrated in the quantizer
            IColorCache result = colorCache ?? (colorCache = OnCreateDefaultCache());

            // if the cache exists; or default one was created for these purposes.. use it
            if (result == null)
            {
                String message = string.Format("The color cache is not initialized! Please use SetColorCache() method on quantizer.");
                throw new ArgumentNullException(message);
            }

            // cache is fine, return it
            return result;
        }

        #endregion

        #region | Abstract/virtual methods |

        /// <summary>
        /// Called when it is needed to create default cache (no cache is supplied from outside).
        /// </summary>
        /// <returns></returns>
        protected abstract IColorCache OnCreateDefaultCache();

        /// <summary>
        /// Redirection to retrieve palette to be cached, if palette is not available yet.
        /// </summary>
        protected abstract List<Color> OnGetPaletteToCache(Int32 colorCount);

        #endregion

        #region << BaseColorCacheQuantizer >>

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnPrepare"/> for more details.
        /// </summary>
        protected override void OnPrepare(Image image)
        {
            base.OnPrepare(image);

            GetColorCache().Prepare();
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnGetPalette"/> for more details.
        /// </summary>
        protected sealed override List<Color> OnGetPalette(Int32 colorCount)
        {
            // use optimization, or calculate new palette if color count is lower than unique color count
            List<Color> palette = base.OnGetPalette(colorCount) ?? OnGetPaletteToCache(colorCount);
            GetColorCache().CachePalette(palette);
            return palette;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnGetPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetPaletteIndex(Color color, Int32 key, Int32 x, Int32 y, out int paletteIndex)
        {
            base.OnGetPaletteIndex(color, key, x, y, out paletteIndex);

            // if not determined, use cache to determine the index
            if (paletteIndex == InvalidIndex)
            {
                GetColorCache().GetColorPaletteIndex(color, out paletteIndex);
            }
        }

        #endregion
    }
    //
    /// <summary>
    /// Stores all the informations about single color only once, to be used later.
    /// </summary>
    public class DistinctColorInfo
    {
        private const Int32 Factor = 5000000;

        /// <summary>
        /// The original color.
        /// </summary>
        public Int32 Color { get; private set; }

        /// <summary>
        /// The pixel presence count in the image.
        /// </summary>
        public Int32 Count { get; private set; }

        /// <summary>
        /// A hue component of the color.
        /// </summary>
        public Int32 Hue { get; private set; }

        /// <summary>
        /// A saturation component of the color.
        /// </summary>
        public Int32 Saturation { get; private set; }

        /// <summary>
        /// A brightness component of the color.
        /// </summary>
        public Int32 Brightness { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctColorInfo"/> struct.
        /// </summary>
        public DistinctColorInfo(Color color)
        {
            Color = color.ToArgb();
            Count = 1;

            Hue = Convert.ToInt32(color.GetHue() * Factor);
            Saturation = Convert.ToInt32(color.GetSaturation() * Factor);
            Brightness = Convert.ToInt32(color.GetBrightness() * Factor);
        }

        /// <summary>
        /// Increases the count of pixels of this color.
        /// </summary>
        public DistinctColorInfo IncreaseCount()
        {
            Count++;
            return this;
        }
    }
    /// <summary>
    /// This is my baby. Read more in the article on the Code Project:
    /// http://www.codeproject.com/KB/recipes/SimplePaletteQuantizer.aspx
    /// </summary>
    public class DistinctSelectionQuantizer : BaseColorCacheQuantizer
    {
        #region | Fields |

        private List<Color> palette;
        private Int32 foundColorCount;
        private ConcurrentDictionary<Int32, DistinctColorInfo> colorMap;

        #endregion

        #region | Methods |

        private static Boolean ProcessList(Int32 colorCount, List<DistinctColorInfo> list, ICollection<IEqualityComparer<DistinctColorInfo>> comparers, out List<DistinctColorInfo> outputList)
        {
            IEqualityComparer<DistinctColorInfo> bestComparer = null;
            Int32 maximalCount = 0;
            outputList = list;

            foreach (IEqualityComparer<DistinctColorInfo> comparer in comparers)
            {
                List<DistinctColorInfo> filteredList = list.
                    Distinct(comparer).
                    ToList();

                Int32 filteredListCount = filteredList.Count;

                if (filteredListCount > colorCount && filteredListCount > maximalCount)
                {
                    maximalCount = filteredListCount;
                    bestComparer = comparer;
                    outputList = filteredList;

                    if (maximalCount <= colorCount) break;
                }
            }

            comparers.Remove(bestComparer);
            return comparers.Count > 0 && maximalCount > colorCount;
        }

        #endregion

        #region << BaseColorCacheQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.Prepare"/> for more details.
        /// </summary>
        protected override void OnPrepare(Image image)
        {
            base.OnPrepare(image);

            OnFinish();
        }

        /// <summary>
        /// See <see cref="BaseColorCacheQuantizer.OnCreateDefaultCache"/> for more details.
        /// </summary>
        protected override IColorCache OnCreateDefaultCache()
        {
            // use OctreeColorCache best performance/quality
            return new OctreeColorCache();
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnAddColor"/> for more details.
        /// </summary>
        protected override void OnAddColor(Color color, Int32 key, Int32 x, Int32 y)
        {
            colorMap.AddOrUpdate(key,
                colorKey => new DistinctColorInfo(color),
                (colorKey, colorInfo) => colorInfo.IncreaseCount());
        }

        /// <summary>
        /// See <see cref="BaseColorCacheQuantizer.OnGetPaletteToCache"/> for more details.
        /// </summary>
        protected override List<Color> OnGetPaletteToCache(Int32 colorCount)
        {
            // otherwise calculate one
            palette.Clear();

            // lucky seed :)
            FastRandom random = new FastRandom(13);
            List<DistinctColorInfo> colorInfoList = colorMap.Values.ToList();

            foundColorCount = colorInfoList.Count;

            if (foundColorCount >= colorCount)
            {
                // shuffles the colormap
                colorInfoList = colorInfoList.
                    OrderBy(entry => random.Next(foundColorCount)).
                    ToList();

                // workaround for backgrounds, the most prevalent color
                DistinctColorInfo background = colorInfoList.MaxBy(info => info.Count);
                colorInfoList.Remove(background);
                colorCount--;

                ColorHueComparer hueComparer = new ColorHueComparer();
                ColorSaturationComparer saturationComparer = new ColorSaturationComparer();
                ColorBrightnessComparer brightnessComparer = new ColorBrightnessComparer();

                // generates catalogue
                List<IEqualityComparer<DistinctColorInfo>> comparers = new List<IEqualityComparer<DistinctColorInfo>> { hueComparer, saturationComparer, brightnessComparer };

                // take adequate number from each slot
                while (ProcessList(colorCount, colorInfoList, comparers, out colorInfoList)) { }

                Int32 listColorCount = colorInfoList.Count();

                if (listColorCount > 0)
                {
                    Int32 allowedTake = Math.Min(colorCount, listColorCount);
                    colorInfoList = colorInfoList.Take(allowedTake).ToList();
                }

                // adds background color first
                palette.Add(Color.FromArgb(background.Color));
            }

            // adds the selected colors to a final palette
            palette.AddRange(colorInfoList.Select(colorInfo => Color.FromArgb(colorInfo.Color)));

            // returns our new palette
            return palette;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.GetColorCount"/> for more details.
        /// </summary>
        protected override Int32 OnGetColorCount()
        {
            return foundColorCount;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnFinish"/> for more details.
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();

            palette = new List<Color>();
            colorMap = new ConcurrentDictionary<Int32, DistinctColorInfo>();
        }

        #endregion

        #region << IColorQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public override Boolean AllowParallel
        {
            get { return true; }
        }

        #endregion

        #region | Helper classes (comparers) |

        /// <summary>
        /// Compares a hue components of a color info.
        /// </summary>
        private class ColorHueComparer : IEqualityComparer<DistinctColorInfo>
        {
            public Boolean Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Hue == y.Hue;
            }

            public Int32 GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Hue.GetHashCode();
            }
        }

        /// <summary>
        /// Compares a saturation components of a color info.
        /// </summary>
        private class ColorSaturationComparer : IEqualityComparer<DistinctColorInfo>
        {
            public Boolean Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Saturation == y.Saturation;
            }

            public Int32 GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Saturation.GetHashCode();
            }
        }

        /// <summary>
        /// Compares a brightness components of a color info.
        /// </summary>
        private class ColorBrightnessComparer : IEqualityComparer<DistinctColorInfo>
        {
            public Boolean Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Brightness == y.Brightness;
            }

            public Int32 GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Brightness.GetHashCode();
            }
        }

        #endregion
    }

    #endregion


    #region Helpers

    #region Pixels

    public interface IIndexedPixel
    {
        // index methods
        Byte GetIndex(Int32 offset);
        void SetIndex(Int32 offset, Byte value);
    }
    public interface INonIndexedPixel
    {
        // components
        Int32 Alpha { get; }
        Int32 Red { get; }
        Int32 Green { get; }
        Int32 Blue { get; }

        // higher-level values
        Int32 Argb { get; }
        UInt64 Value { get; set; }

        // color methods
        Color GetColor();
        void SetColor(Color color);
    }
    #region Indexed

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData1Indexed : IIndexedPixel
    {
        // raw component values
        private Byte index;

        // get - index method
        public Byte GetIndex(Int32 offset)
        {
            return (index & 1 << (7 - offset)) != 0 ? Pixel.One : Pixel.Zero;
        }

        // set - index method
        public void SetIndex(Int32 offset, Byte value)
        {
            value = value == 0 ? Pixel.One : Pixel.Zero;

            if (value == 0)
            {
                index |= (Byte)(1 << (7 - offset));
            }
            else
            {
                index &= (Byte)(~(1 << (7 - offset)));
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData4Indexed : IIndexedPixel
    {
        // raw component values
        private Byte index;

        // get - index method
        public Byte GetIndex(Int32 offset)
        {
            return (Byte)GetBitRange(8 - offset - 4, 7 - offset);
        }

        // set - index method
        public void SetIndex(Int32 offset, Byte value)
        {
            SetBitRange(8 - offset - 4, 7 - offset, value);
        }

        private Int32 GetBitRange(Int32 startOffset, Int32 endOffset)
        {
            Int32 result = 0;
            Byte bitIndex = 0;

            for (Int32 offset = startOffset; offset <= endOffset; offset++)
            {
                Int32 bitValue = 1 << bitIndex;
                result += GetBit(offset) ? bitValue : 0;
                bitIndex++;
            }

            return result;
        }

        private Boolean GetBit(Int32 offset)
        {
            return (index & (1 << offset)) != 0;
        }

        private void SetBitRange(Int32 startOffset, Int32 endOffset, Int32 value)
        {
            Byte bitIndex = 0;

            for (Int32 offset = startOffset; offset <= endOffset; offset++)
            {
                Int32 bitValue = 1 << bitIndex;
                SetBit(offset, (value & bitValue) != 0);
                bitIndex++;
            }
        }

        private void SetBit(Int32 offset, Boolean value)
        {
            if (value)
            {
                index |= (Byte)(1 << offset);
            }
            else
            {
                //int offsetB = (~(1 << offset));
                index &= (Byte)(~(1 << offset));
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData8Indexed : IIndexedPixel
    {
        // raw component values
        private Byte index;

        // index methods 
        public Byte GetIndex(Int32 offset) { return index; }
        public void SetIndex(Int32 offset, Byte value) { index = value; }
    }
    #endregion

    #region NonIndexed

    /// <summary>
    /// Name |     Blue     |    Green     |     Red      | Alpha (bit)
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataArgb1555 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private readonly Byte blue;     // 00 - 04
        [FieldOffset(0)]
        private readonly UInt16 green;  // 05 - 09
        [FieldOffset(1)]
        private readonly Byte red;      // 10 - 14
        [FieldOffset(1)]
        private readonly Byte alpha;    // 15

        // raw high-level values
        [FieldOffset(0)]
        private UInt16 raw;    // 00 - 15

        // processed raw values
        public Int32 Alpha { get { return (alpha >> 7) & 0x1; } }
        public Int32 Red { get { return (red >> 2) & 0xF; } }
        public Int32 Green { get { return (green >> 5) & 0xF; } }
        public Int32 Blue { get { return blue & 0xF; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return (Alpha == 0 ? 0 : Pixel.AlphaMask) | Red << Pixel.RedShift | Green << Pixel.GreenShift | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            raw = (UInt16)((color.A < 255 ? Pixel.Zero : Pixel.One) << 15 | (color.R >> 3) << 10 | (color.G >> 3) << 5 | (color.B >> 3));
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return raw; }
            set { raw = (UInt16)(value & 0xFFFF); }
        }
    }
    /// <summary>
    /// Name |                     Blue                      |                     Green                     |                      Red                      |                     Alpha                     |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|60|61|62|63|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|33333333333333333333333|44444444444444444444444|55555555555555555555555|66666666666666666666666|77777777777777777777777|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct PixelDataArgb64 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private UInt16 blue;   // 00 - 15
        [FieldOffset(2)]
        private UInt16 green;  // 16 - 31
        [FieldOffset(4)]
        private UInt16 red;    // 32 - 47
        [FieldOffset(6)]
        private UInt16 alpha;  // 48 - 63

        // raw high-level values
        [FieldOffset(0)]
        private UInt64 raw;    // 00 - 63

        // processed component values
        public Int32 Alpha { get { return alpha >> 5; } }
        public Int32 Red { get { return red >> 5; } }
        public Int32 Green { get { return green >> 5; } }
        public Int32 Blue { get { return blue >> 5; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Alpha << 48 | Red << 32 | Green << 16 | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            alpha = (UInt16)(color.A << 5);
            red = (UInt16)(color.R << 5);
            green = (UInt16)(color.G << 5);
            blue = (UInt16)(color.B << 5);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return raw; }
            set { raw = value; }
        }
    }
    /// <summary>
    /// Name |          Blue         |        Green          |           Red         |         Alpha         |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|33333333333333333333333|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct PixelDataArgb8888 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private readonly Byte blue;    // 00 - 07
        [FieldOffset(1)]
        private readonly Byte green;   // 08 - 15
        [FieldOffset(2)]
        private readonly Byte red;     // 16 - 23
        [FieldOffset(3)]
        private readonly Byte alpha;   // 24 - 31

        // raw high-level values
        [FieldOffset(0)]
        private Int32 raw;             // 00 - 31

        // processed component values
        public Int32 Alpha { get { return alpha; } }
        public Int32 Red { get { return red; } }
        public Int32 Green { get { return green; } }
        public Int32 Blue { get { return blue; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            raw = color.ToArgb();
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return (UInt32)raw; }
            set { raw = (Int32)(value & 0xFFFFFFFF); }
        }
    }
    /// <summary>
    /// Name |                  Grayscale                    |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15| 
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataGray16 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private UInt16 gray;   // 00 - 15

        // processed raw values
        public Int32 Gray { get { return (0xFF >> 8) & 0xF; } }
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return Gray; } }
        public Int32 Green { get { return Gray; } }
        public Int32 Blue { get { return Gray; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return (Pixel.AlphaMask) | Red << Pixel.RedShift | Green << Pixel.GreenShift | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            Int32 argb = color.ToArgb() & Pixel.RedGreenBlueMask;
            gray = (Byte)(argb >> Pixel.RedShift);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return gray; }
            set { gray = (UInt16)(value & 0xFFFF); }
        }
    }
    /// <summary>
    /// Name |                     Blue                      |                     Green                     |                      Red                      | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|33333333333333333333333|44444444444444444444444|55555555555555555555555|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public struct PixelDataRgb48 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private UInt16 blue;   // 00 - 15
        [FieldOffset(2)]
        private UInt16 green;  // 16 - 31
        [FieldOffset(4)]
        private UInt16 red;    // 32 - 47

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return red >> 5; } }
        public Int32 Green { get { return green >> 5; } }
        public Int32 Blue { get { return blue >> 5; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Alpha << 48 | Red << 32 | Green << 16 | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            red = (UInt16)(color.R << 5);
            green = (UInt16)(color.G << 5);
            blue = (UInt16)(color.B << 5);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return (UInt64)(red << 32 | green << 16 | blue); }
            set
            {
                red = (UInt16)((value >> 32) & 0xFFFF);
                green = (UInt16)((value >> 16) & 0xFFFF);
                blue = (UInt16)(value & 0xFFFF);
            }
        }
    }
    /// <summary>
    /// Name |     Blue     |    Green     |     Red      | Unused
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataRgb555 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private readonly Byte blue;     // 00 - 04
        [FieldOffset(0)]
        private readonly UInt16 green;  // 05 - 09
        [FieldOffset(1)]
        private readonly Byte red;      // 10 - 14

        // raw high-level values
        [FieldOffset(0)]
        private UInt16 raw;    // 00 - 15

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return (red >> 2) & 0xF; } }
        public Int32 Green { get { return (green >> 5) & 0xF; } }
        public Int32 Blue { get { return blue & 0xF; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Pixel.AlphaMask | raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            raw = (UInt16)((color.R >> 3) << 10 | (color.G >> 3) << 5 | color.B >> 3);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return raw; }
            set { raw = (UInt16)(value & 0xFFFF); }
        }
    }
    /// <summary>
    /// Name |     Blue     |      Green      |     Red      | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataRgb565 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private readonly Byte blue;     // 00 - 04
        [FieldOffset(0)]
        private readonly UInt16 green;  // 05 - 10
        [FieldOffset(1)]
        private readonly Byte red;      // 11 - 15

        // raw high-level values
        [FieldOffset(0)]
        private UInt16 raw;    // 00 - 15

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return (red >> 3) & 0xF; } }
        public Int32 Green { get { return (green >> 5) & 0x1F; } }
        public Int32 Blue { get { return blue & 0xF; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Pixel.AlphaMask | raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            raw = (UInt16)((color.R >> 3) << 11 | (color.G >> 2) << 5 | color.B >> 3);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return raw; }
            set { raw = (UInt16)(value & 0xFFFF); }
        }
    }
    /// <summary>
    /// Name |          Blue         |        Green          |           Red         | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct PixelDataRgb888 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private Byte blue;    // 00 - 07
        [FieldOffset(1)]
        private Byte green;   // 08 - 15
        [FieldOffset(2)]
        private Byte red;     // 16 - 23

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return red; } }
        public Int32 Green { get { return green; } }
        public Int32 Blue { get { return blue; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Pixel.AlphaMask | Red << Pixel.RedShift | Green << Pixel.GreenShift | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            red = color.R;
            green = color.G;
            blue = color.B;
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return (UInt32)Argb; }
            set
            {
                red = (Byte)((value >> Pixel.RedShift) & 0xFF);
                green = (Byte)((value >> Pixel.GreenShift) & 0xFF);
                blue = (Byte)((value >> Pixel.BlueShift) & 0xFF);
            }
        }
    }
    /// <summary>
    /// Name |          Blue         |        Green          |           Red         |         Unused        |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|33333333333333333333333|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct PixelDataRgb8888 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)]
        private readonly Byte blue;    // 00 - 07
        [FieldOffset(1)]
        private readonly Byte green;   // 08 - 15
        [FieldOffset(2)]
        private readonly Byte red;     // 16 - 23
        [FieldOffset(3)]
        private readonly Byte unused;  // 24 - 31

        // raw high-level values
        [FieldOffset(0)]
        private Int32 raw;             // 00 - 23

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return red; } }
        public Int32 Green { get { return green; } }
        public Int32 Blue { get { return blue; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Pixel.AlphaMask | raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            raw = color.ToArgb() & Pixel.RedGreenBlueMask;
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return (UInt32)raw; }
            set { raw = (Int32)(value & 0xFFFFFF); }
        }
    }

    #endregion

    #endregion

    public class ColorModelHelper
    {
        #region | Constants |

        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;

        private const float Epsilon = 1E-05f;
        private const float OneThird = 1.0f / 3.0f;
        private const float TwoThirds = 2.0f * OneThird;
        public const Double HueFactor = 1.4117647058823529411764705882353;

        private static readonly float[] XYZWhite = new[] { 95.05f, 100.00f, 108.90f };

        private static readonly float[,] Rgb2Xyz = 
        { 
            { 0.41239083F, 0.35758433F, 0.18048081F },
	        { 0.21263903F, 0.71516865F, 0.072192319F },
	        { 0.019330820F, 0.11919473F, 0.95053220F }
        };

        private static readonly float[,] Xyz2Rgb = 
        {
	        { 3.2409699F, -1.5373832F, -0.49861079F },
	        { -0.96924376F, 1.8759676F, 0.041555084F },
	        { 0.055630036F, -0.20397687F, 1.0569715F }
        };

        #endregion

        #region | -> RGB |

        private static Int32 GetColorComponent(Single v1, Single v2, Single hue)
        {
            Single preresult;

            if (hue < 0.0f) hue++;
            if (hue > 1.0f) hue--;

            if ((6.0f * hue) < 1.0f)
            {
                preresult = v1 + (((v2 - v1) * 6.0f) * hue);
            }
            else if ((2.0f * hue) < 1.0f)
            {
                preresult = v2;
            }
            else if ((3.0f * hue) < 2.0f)
            {
                preresult = v1 + (((v2 - v1) * (TwoThirds - hue)) * 6.0f);
            }
            else
            {
                preresult = v1;
            }

            return Convert.ToInt32(255.0f * preresult);
        }

        public static Color HSBtoRGB(Single hue, Single saturation, Single brightness)
        {
            // initializes the default black
            Int32 red = 0;
            Int32 green = 0;
            Int32 blue = 0;

            // only if there is some brightness; otherwise leave it pitch black
            if (brightness > 0.0f)
            {
                // if there is no saturation; leave it gray based on the brightness only
                if (Math.Abs(saturation - 0.0f) < Epsilon)
                {
                    red = green = blue = Convert.ToInt32(255.0f * brightness);
                }
                else // the color is more complex
                {
                    // converts HSL cylinder to one slice (its factors)
                    Single factorHue = hue / 360.0f;
                    Single factorA = brightness < 0.5f ? brightness * (1.0f + saturation) : (brightness + saturation) - (brightness * saturation);
                    Single factorB = (2.0f * brightness) - factorA;

                    // maps HSL slice to a RGB cube
                    red = GetColorComponent(factorB, factorA, factorHue + OneThird);
                    green = GetColorComponent(factorB, factorA, factorHue);
                    blue = GetColorComponent(factorB, factorA, factorHue - OneThird);
                }
            }

            Int32 argb = 255 << 24 | red << 16 | green << 8 | blue;
            return Color.FromArgb(argb);
        }

        #endregion

        #region | RGB -> |

        public static void RGBtoLab(Int32 red, Int32 green, Int32 blue, out Single l, out Single a, out Single b)
        {
            Single x, y, z;
            RGBtoXYZ(red, green, blue, out x, out y, out z);
            XYZtoLab(x, y, z, out l, out a, out b);
        }

        public static void RGBtoXYZ(Int32 red, Int32 green, Int32 blue, out Single x, out Single y, out Single z)
        {
            // normalize red, green, blue values
            Double redFactor = red / 255.0;
            Double greenFactor = green / 255.0;
            Double blueFactor = blue / 255.0;

            // convert to a sRGB form
            Double sRed = (redFactor > 0.04045) ? Math.Pow((redFactor + 0.055) / (1 + 0.055), 2.2) : (redFactor / 12.92);
            Double sGreen = (greenFactor > 0.04045) ? Math.Pow((greenFactor + 0.055) / (1 + 0.055), 2.2) : (greenFactor / 12.92);
            Double sBlue = (blueFactor > 0.04045) ? Math.Pow((blueFactor + 0.055) / (1 + 0.055), 2.2) : (blueFactor / 12.92);

            // converts
            x = Convert.ToSingle(sRed * 0.4124 + sGreen * 0.3576 + sBlue * 0.1805);
            y = Convert.ToSingle(sRed * 0.2126 + sGreen * 0.7152 + sBlue * 0.0722);
            z = Convert.ToSingle(sRed * 0.0193 + sGreen * 0.1192 + sBlue * 0.9505);
        }

        #endregion

        #region | XYZ -> |

        private static Single GetXYZValue(Single value)
        {
            return value > 0.008856f ? (Single)Math.Pow(value, OneThird) : (7.787f * value + 16.0f / 116.0f);
        }

        public static void XYZtoLab(Single x, Single y, Single z, out Single l, out Single a, out Single b)
        {
            l = 116.0f * GetXYZValue(y / XYZWhite[Y]) - 16.0f;
            a = 500.0f * (GetXYZValue(x / XYZWhite[X]) - GetXYZValue(y / XYZWhite[Y]));
            b = 200.0f * (GetXYZValue(y / XYZWhite[Y]) - GetXYZValue(z / XYZWhite[Z]));
        }

        #endregion

        #region | Methods |

        public static Int64 GetColorEuclideanDistance(ColorModel colorModel, Color requestedColor, Color realColor)
        {
            Single componentA, componentB, componentC;
            GetColorComponents(colorModel, requestedColor, realColor, out componentA, out componentB, out componentC);
            return (Int64)(componentA * componentA + componentB * componentB + componentC * componentC);
        }

        public static Int32 GetEuclideanDistance(Color color, ColorModel colorModel, IList<Color> palette)
        {
            // initializes the best difference, set it for worst possible, it can only get better
            Int64 leastDistance = Int64.MaxValue;
            Int32 result = 0;

            for (Int32 index = 0; index < palette.Count; index++)
            {
                Color targetColor = palette[index];
                Int64 distance = GetColorEuclideanDistance(colorModel, color, targetColor);

                // if a difference is zero, we're good because it won't get better
                if (distance == 0)
                {
                    result = index;
                    break;
                }

                // if a difference is the best so far, stores it as our best candidate
                if (distance < leastDistance)
                {
                    leastDistance = distance;
                    result = index;
                }
            }

            return result;
        }

        public static Int32 GetComponentA(ColorModel colorModel, Color color)
        {
            Int32 result = 0;

            switch (colorModel)
            {
                case ColorModel.RedGreenBlue:
                    result = color.R;
                    break;

                case ColorModel.HueSaturationBrightness:
                    result = Convert.ToInt32(color.GetHue() / HueFactor);
                    break;

                case ColorModel.LabColorSpace:
                    Single l, a, b;
                    RGBtoLab(color.R, color.G, color.B, out l, out a, out b);
                    result = Convert.ToInt32(l * 255.0f);
                    break;
            }

            return result;
        }

        public static Int32 GetComponentB(ColorModel colorModel, Color color)
        {
            Int32 result = 0;

            switch (colorModel)
            {
                case ColorModel.RedGreenBlue:
                    result = color.G;
                    break;

                case ColorModel.HueSaturationBrightness:
                    result = Convert.ToInt32(color.GetSaturation() * 255);
                    break;

                case ColorModel.LabColorSpace:
                    Single l, a, b;
                    RGBtoLab(color.R, color.G, color.B, out l, out a, out b);
                    result = Convert.ToInt32(a * 255.0f);
                    break;
            }

            return result;
        }

        public static Int32 GetComponentC(ColorModel colorModel, Color color)
        {
            Int32 result = 0;

            switch (colorModel)
            {
                case ColorModel.RedGreenBlue:
                    result = color.B;
                    break;

                case ColorModel.HueSaturationBrightness:
                    result = Convert.ToInt32(color.GetBrightness() * 255);
                    break;

                case ColorModel.LabColorSpace:
                    Single l, a, b;
                    RGBtoLab(color.R, color.G, color.B, out l, out a, out b);
                    result = Convert.ToInt32(b * 255.0f);
                    break;
            }

            return result;
        }

        public static void GetColorComponents(ColorModel colorModel, Color color, out Single componentA, out Single componentB, out Single componentC)
        {
            componentA = 0.0f;
            componentB = 0.0f;
            componentC = 0.0f;

            switch (colorModel)
            {
                case ColorModel.RedGreenBlue:
                    componentA = color.R;
                    componentB = color.G;
                    componentC = color.B;
                    break;

                case ColorModel.HueSaturationBrightness:
                    componentA = color.GetHue();
                    componentB = color.GetSaturation();
                    componentC = color.GetBrightness();
                    break;

                case ColorModel.LabColorSpace:
                    RGBtoLab(color.R, color.G, color.B, out componentA, out componentB, out componentC);
                    break;

                case ColorModel.XYZ:
                    RGBtoXYZ(color.R, color.G, color.B, out componentA, out componentB, out componentC);
                    break;
            }
        }

        public static void GetColorComponents(ColorModel colorModel, Color color, Color targetColor, out Single componentA, out Single componentB, out Single componentC)
        {
            componentA = 0.0f;
            componentB = 0.0f;
            componentC = 0.0f;

            switch (colorModel)
            {
                case ColorModel.RedGreenBlue:
                    componentA = color.R - targetColor.R;
                    componentB = color.G - targetColor.G;
                    componentC = color.B - targetColor.B;
                    break;

                case ColorModel.HueSaturationBrightness:
                    componentA = color.GetHue() - targetColor.GetHue();
                    componentB = color.GetSaturation() - targetColor.GetSaturation();
                    componentC = color.GetBrightness() - targetColor.GetBrightness();
                    break;

                case ColorModel.LabColorSpace:

                    Single sourceL, sourceA, sourceB;
                    Single targetL, targetA, targetB;

                    RGBtoLab(color.R, color.G, color.B, out sourceL, out sourceA, out sourceB);
                    RGBtoLab(targetColor.R, targetColor.G, targetColor.B, out targetL, out targetA, out targetB);

                    componentA = sourceL - targetL;
                    componentB = sourceA - targetA;
                    componentC = sourceB - targetB;

                    break;

                case ColorModel.XYZ:

                    Single sourceX, sourceY, sourceZ;
                    Single targetX, targetY, targetZ;

                    RGBtoXYZ(color.R, color.G, color.B, out sourceX, out sourceY, out sourceZ);
                    RGBtoXYZ(targetColor.R, targetColor.G, targetColor.B, out targetX, out targetY, out targetZ);

                    componentA = sourceX - targetX;
                    componentB = sourceY - targetY;
                    componentC = sourceZ - targetZ;

                    break;
            }
        }

        public static void HighColorAmplification(ref Int32 red, ref Int32 green, ref Int32 blue)
        {
            Single redfloatValue = red / 8192.0f;
            Single greenfloatValue = green / 8192.0f;
            Single bluefloatValue = blue / 8192.0f;

            Single[] result = new Single[3];

            for (Int32 index = 0; index < 3; index++)
            {
                result[index] += Rgb2Xyz[index, 0] * redfloatValue;
                result[index] += Rgb2Xyz[index, 1] * greenfloatValue;
                result[index] += Rgb2Xyz[index, 2] * bluefloatValue;
            }

            Single x = result[0] + result[1] + result[2];
            Single y = result[1];

            if (x > 0)
            {
                redfloatValue = y;
                greenfloatValue = result[0] / x;
                bluefloatValue = result[1] / x;
            }
            else
            {
                redfloatValue = 0.0f;
                greenfloatValue = 0.0f;
                bluefloatValue = 0.0f;
            }

            Single bias = (Single)(Math.Log(0.85f) / -0.693147f);
            Single exposure = 1.0f;
            Single lumAvg = redfloatValue;
            Single lumMax = redfloatValue;
            Single lumNormal = lumMax / lumAvg;
            Single divider = (Single)Math.Log10(lumNormal + 1.0);

            Double yw = (redfloatValue / lumAvg) * exposure;
            Double interpol = Math.Log(2 + Math.Pow(yw / lumNormal, bias) * 8);
            Double l = PadeLog(yw);
            redfloatValue = (Single)((l / interpol) / divider);

            Single z;
            y = redfloatValue;
            Array.Clear(result, 0, 3);

            result[1] = greenfloatValue;
            result[2] = bluefloatValue;

            if ((y > 1e-06f) && (result[1] > 1e-06F) && (result[2] > 1e-06F))
            {
                x = (result[1] * y) / result[2];
                z = (x / result[1]) - x - y;
            }
            else
            {
                x = z = 1e-06F;
            }

            redfloatValue = x;
            greenfloatValue = y;
            bluefloatValue = z;

            Array.Clear(result, 0, 3);

            for (int i = 0; i < 3; i++)
            {
                result[i] += Xyz2Rgb[i, 0] * redfloatValue;
                result[i] += Xyz2Rgb[i, 1] * greenfloatValue;
                result[i] += Xyz2Rgb[i, 2] * bluefloatValue;
            }

            redfloatValue = result[0] > 1 ? 1 : result[0];
            greenfloatValue = result[1] > 1 ? 1 : result[1];
            bluefloatValue = result[2] > 1 ? 1 : result[2];

            red = (Byte)(255 * redfloatValue + 0.5);
            green = (Byte)(255 * greenfloatValue + 0.5);
            blue = (Byte)(255 * bluefloatValue + 0.5);
        }

        private static Double PadeLog(Double value)
        {
            if (value < 1)
            {
                return (value * (6 + value) / (6 + 4 * value));
            }

            if (value < 2)
            {
                return (value * (6 + 0.7662 * value) / (5.9897 + 3.7658 * value));
            }

            return Math.Log(value + 1);
        }

        #endregion
    }
    public class FastRandom
    {
        private const Double RealUnitInt = 1.0 / (Int32.MaxValue + 1.0);

        private UInt32 x, y, z, w;

        public FastRandom(UInt32 seed)
        {
            x = seed;
            y = 842502087;
            z = 3579807591;
            w = 273326509;
        }

        public Int32 Next(Int32 upperBound)
        {
            UInt32 t = (x ^ (x << 11)); x = y; y = z; z = w;
            return (Int32)((RealUnitInt * (Int32)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * upperBound);
        }
    }
    public static class Guard
    {
        /// <summary>
        /// Checks if an argument is null
        /// </summary>
        /// <param name="argument">argument</param>
        /// <param name="argumentName">argument name</param>
        public static void CheckNull(Object argument, String argumentName)
        {
            if (argument == null)
            {
                String message = string.Format("Cannot use '{0}' when it is null!", argumentName);
                throw new ArgumentNullException(message);
            }
        }
    }
    public class ImageBuffer : IDisposable
    {
        #region | Fields |

        private Int32[] fastBitX;
        private Int32[] fastByteX;
        private Int32[] fastY;

        private readonly Bitmap bitmap;
        private readonly BitmapData bitmapData;
        private readonly ImageLockMode lockMode;

        private List<Color> cachedPalette;

        #endregion

        #region | Delegates |

        public delegate Boolean ProcessPixelFunction(Int32 passIndex, Pixel pixel);
        public delegate Boolean ProcessPixelAdvancedFunction(Int32 passIndex, Pixel pixel, ImageBuffer buffer);
        public delegate Boolean TransformPixelFunction(Int32 passIndex, Pixel sourcePixel, Pixel targetPixel);
        public delegate Boolean TransformPixelAdvancedFunction(Int32 passIndex, Pixel sourcePixel, Pixel targetPixel, ImageBuffer sourceBuffer, ImageBuffer targetBuffer);

        #endregion

        #region | Properties |

        public Int32 Width { get; private set; }
        public Int32 Height { get; private set; }

        public Int32 Size { get; private set; }
        public Int32 Stride { get; private set; }
        public Int32 BitDepth { get; private set; }
        public Int32 BytesPerPixel { get; private set; }

        public Boolean IsIndexed { get; private set; }
        public PixelFormat PixelFormat { get; private set; }

        #endregion

        #region | Calculated properties |

        /// <summary>
        /// Gets a value indicating whether this buffer can be read.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can read; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanRead
        {
            get { return lockMode == ImageLockMode.ReadOnly || lockMode == ImageLockMode.ReadWrite; }
        }

        /// <summary>
        /// Gets a value indicating whether this buffer can written to.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can write; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanWrite
        {
            get { return lockMode == ImageLockMode.WriteOnly || lockMode == ImageLockMode.ReadWrite; }
        }

        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        public List<Color> Palette
        {
            get { return UpdatePalette(); }
            set
            {
                bitmap.SetPalette(value);
                cachedPalette = value;
            }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBuffer"/> class.
        /// </summary>
        public ImageBuffer(Image bitmap, ImageLockMode lockMode) : this((Bitmap)bitmap, lockMode) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBuffer"/> class.
        /// </summary>
        public ImageBuffer(Bitmap bitmap, ImageLockMode lockMode)
        {
            // locks the image data
            this.bitmap = bitmap;
            this.lockMode = lockMode;

            // gathers the informations
            Width = bitmap.Width;
            Height = bitmap.Height;
            PixelFormat = bitmap.PixelFormat;
            IsIndexed = PixelFormat.IsIndexed();
            BitDepth = PixelFormat.GetBitDepth();
            BytesPerPixel = Math.Max(1, BitDepth >> 3);

            // determines the bounds of an image, and locks the data in a specified mode
            Rectangle bounds = Rectangle.FromLTRB(0, 0, Width, Height);

            // locks the bitmap data
            lock (bitmap) bitmapData = bitmap.LockBits(bounds, lockMode, PixelFormat);

            // creates internal buffer
            Stride = bitmapData.Stride < 0 ? -bitmapData.Stride : bitmapData.Stride;
            Size = Stride * Height;

            // precalculates the offsets
            Precalculate();
        }

        #endregion

        #region | Maintenance methods |

        private void Precalculate()
        {
            fastBitX = new Int32[Width];
            fastByteX = new Int32[Width];
            fastY = new Int32[Height];

            // precalculates the x-coordinates
            for (Int32 x = 0; x < Width; x++)
            {
                fastBitX[x] = x * BitDepth;
                fastByteX[x] = fastBitX[x] >> 3;
                fastBitX[x] = fastBitX[x] % 8;
            }

            // precalculates the y-coordinates
            for (Int32 y = 0; y < Height; y++)
            {
                fastY[y] = y * bitmapData.Stride;
            }
        }

        public Int32 GetBitOffset(Int32 x)
        {
            return fastBitX[x];
        }

        public Byte[] Copy()
        {
            // transfers whole image to a working memory
            Byte[] result = new Byte[Size];
            Marshal.Copy(bitmapData.Scan0, result, 0, Size);

            // returns the backup
            return result;
        }

        public void Paste(Byte[] buffer)
        {
            // commits the data to a bitmap
            Marshal.Copy(buffer, 0, bitmapData.Scan0, Size);
        }

        #endregion

        #region | Pixel methods |

        /// <summary>
        /// Create a pixel in 
        /// </summary>
        /// <returns></returns>
        public Pixel CreatePixel()
        {
            return new Pixel(this);
        }

        #endregion

        #region | Pixel read methods |

        public void ReadPixel(Pixel pixel, Byte[] buffer = null)
        {
            // determines pixel offset at [x, y]
            Int32 offset = fastByteX[pixel.X] + fastY[pixel.Y];

            // reads the pixel from a bitmap
            if (buffer == null)
            {
                pixel.ReadRawData(bitmapData.Scan0 + offset);
            }
            else // reads the pixel from a buffer
            {
                pixel.ReadData(buffer, offset);
            }
        }

        public Int32 ReadIndexUsingPixel(Pixel pixel, Byte[] buffer = null)
        {
            // reads the pixel from bitmap/buffer
            ReadPixel(pixel, buffer);

            // returns the found color
            return pixel.GetIndex();
        }

        public Color ReadColorUsingPixel(Pixel pixel, Byte[] buffer = null)
        {
            // reads the pixel from bitmap/buffer
            ReadPixel(pixel, buffer);

            // returns the found color
            return pixel.GetColor();
        }

        public Int32 ReadIndexUsingPixelFrom(Pixel pixel, Int32 x, Int32 y, Byte[] buffer = null)
        {
            // redirects pixel -> [x, y]
            pixel.Update(x, y);

            // reads index from a bitmap/buffer using pixel, and stores it in the pixel
            return ReadIndexUsingPixel(pixel, buffer);
        }

        public Color ReadColorUsingPixelFrom(Pixel pixel, Int32 x, Int32 y, Byte[] buffer = null)
        {
            // redirects pixel -> [x, y]
            pixel.Update(x, y);

            // reads color from a bitmap/buffer using pixel, and stores it in the pixel
            return ReadColorUsingPixel(pixel, buffer);
        }

        #endregion

        #region | Pixel write methods |

        private void WritePixel(Pixel pixel, Byte[] buffer = null)
        {
            // determines pixel offset at [x, y]
            Int32 offset = fastByteX[pixel.X] + fastY[pixel.Y];

            // writes the pixel to a bitmap
            if (buffer == null)
            {
                pixel.WriteRawData(bitmapData.Scan0 + offset);
            }
            else // writes the pixel to a buffer
            {
                pixel.WriteData(buffer, offset);
            }
        }

        public void WriteIndexUsingPixel(Pixel pixel, Int32 index, Byte[] buffer = null)
        {
            // checks parameters
            Guard.CheckNull(pixel, "pixel");

            // sets index to pixel (pixel's index is updated)
            pixel.SetIndex(index, buffer);

            // writes pixel to a bitmap/buffer
            WritePixel(pixel, buffer);
        }

        public void WriteColorUsingPixel(Pixel pixel, Color color, IColorQuantizer quantizer, Byte[] buffer = null)
        {
            // checks parameters
            Guard.CheckNull(pixel, "pixel");

            // sets color to pixel (pixel is updated with color)
            pixel.SetColor(color, quantizer);

            // writes pixel to a bitmap/buffer
            WritePixel(pixel, buffer);
        }

        public void WriteIndexUsingPixelAt(Pixel pixel, Int32 x, Int32 y, Int32 index, Byte[] buffer = null)
        {
            // redirects pixel -> [x, y]
            pixel.Update(x, y);

            // writes color to bitmap/buffer using pixel
            WriteIndexUsingPixel(pixel, index, buffer);
        }

        public void WriteColorUsingPixelAt(Pixel pixel, Int32 x, Int32 y, Color color, IColorQuantizer quantizer, Byte[] buffer = null)
        {
            // redirects pixel -> [x, y]
            pixel.Update(x, y);

            // writes color to bitmap/buffer using pixel
            WriteColorUsingPixel(pixel, color, quantizer, buffer);
        }

        #endregion

        #region | Generic methods |

        private void ProcessInParallel(Int32 passIndex, Action<LineTask> process, ICollection<Point> path = null, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(process, "process");

            // updates the palette
            UpdatePalette();

            // prepares parallel processing
            Double pointsPerTask = (1.0 * path.Count) / parallelTaskCount;
            LineTask[] lineTasks = new LineTask[parallelTaskCount];
            Double pointOffset = 0.0;

            // creates task for each batch of rows
            for (Int32 index = 0; index < parallelTaskCount; index++)
            {
                lineTasks[index] = new LineTask(passIndex, (Int32)pointOffset, (Int32)(pointOffset + pointsPerTask));
                pointOffset += pointsPerTask;
            }

            // process the image in a parallel manner
            Parallel.ForEach(lineTasks, process);
        }

        #endregion

        #region | Processing methods |

        private void ProcessPerPixelBase(IList<Point> path = null, Int32 parallelTaskCount = 4, params Delegate[] passes)
        {
            // checks parameters
            Guard.CheckNull(passes, "passes");

            // determines mode
            Boolean isAdvanced = passes.Length > 0 && passes[0] is ProcessPixelAdvancedFunction;

            // creates default path, if none is available
            if (path == null) path = StandardPathProvider.CreatePath(Width, Height);

            // prepares the per pixel task
            Action<LineTask> processPerPixel = lineTask =>
            {
                // initializes variables per task
                Pixel pixel = CreatePixel();
                Delegate pass = passes[lineTask.PassIndex];

                for (Int32 pathOffset = lineTask.StartOffset; pathOffset < lineTask.EndOffset; pathOffset++)
                {
                    Point point = path[pathOffset];
                    Boolean allowWrite;

                    // enumerates the pixel, and returns the control to the outside
                    pixel.Update(point.X, point.Y);

                    // when read is allowed, retrieves current value (in bytes)
                    if (CanRead) ReadPixel(pixel);

                    // process the pixel by custom user operation
                    if (isAdvanced)
                    {
                        ProcessPixelAdvancedFunction processAdvancedFunction = (ProcessPixelAdvancedFunction)pass;
                        allowWrite = processAdvancedFunction(lineTask.PassIndex, pixel, this);
                    }
                    else // use simplified version with pixel parameter only
                    {
                        ProcessPixelFunction processFunction = (ProcessPixelFunction)pass;
                        allowWrite = processFunction(lineTask.PassIndex, pixel);
                    }

                    // when write is allowed, copies the value back to the row buffer
                    if (CanWrite && allowWrite) WritePixel(pixel);
                }
            };

            // processes image per pixel
            for (Int32 passIndex = 0; passIndex < passes.Length; passIndex++)
            {
                ProcessInParallel(passIndex, processPerPixel, path, parallelTaskCount);
            }
        }

        public void ProcessPerPixel(IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelFunction[] passes)
        {
            ProcessPerPixelBase(path, parallelTaskCount, passes);
        }

        public void ProcessPerPixel(IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelAdvancedFunction[] passes)
        {
            ProcessPerPixelBase(path, parallelTaskCount, passes);
        }

        public static void ProcessPerPixel(ImageBuffer source, IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelFunction[] passes)
        {
            source.ProcessPerPixelBase(path, parallelTaskCount, passes);
        }

        public static void ProcessPerPixel(ImageBuffer source, IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelAdvancedFunction[] passes)
        {
            source.ProcessPerPixelBase(path, parallelTaskCount, passes);
        }

        public static void ProcessPerPixel(Image sourceImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                source.ProcessPerPixelBase(path, parallelTaskCount, passes);
            }
        }

        public static void ProcessPerPixel(Image sourceImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params ProcessPixelAdvancedFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                source.ProcessPerPixelBase(path, parallelTaskCount, passes);
            }
        }

        #endregion

        #region | Transformation functions |

        private void TransformPerPixelBase(ImageBuffer target, IList<Point> path = null, Int32 parallelTaskCount = 4, params Delegate[] passes)
        {
            // checks parameters
            Guard.CheckNull(target, "target");
            Guard.CheckNull(passes, "passes");

            // creates internal quantizer if needed
            UpdatePalette();
            target.UpdatePalette();

            // checks the dimensions
            if (Width != target.Width || Height != target.Height)
            {
                const String message = "Both images have to have the same dimensions.";
                throw new ArgumentOutOfRangeException(message);
            }

            // determines mode
            Boolean isAdvanced = passes.Length > 0 && passes[0] is TransformPixelAdvancedFunction;

            // creates default path, if none is available
            if (path == null) path = StandardPathProvider.CreatePath(Width, Height);

            // process the image in a parallel manner
            Action<LineTask> transformPerPixel = lineTask =>
            {
                // creates individual pixel structures per task
                Pixel sourcePixel = CreatePixel();
                Pixel targetPixel = target.CreatePixel();
                Delegate pass = passes[lineTask.PassIndex];

                // enumerates the pixels row by row
                for (Int32 pathOffset = lineTask.StartOffset; pathOffset < lineTask.EndOffset; pathOffset++)
                {
                    Point point = path[pathOffset];
                    Boolean allowWrite;

                    // enumerates the pixel, and returns the control to the outside
                    sourcePixel.Update(point.X, point.Y);
                    targetPixel.Update(point.X, point.Y);

                    // when read is allowed, retrieves current value (in bytes)
                    if (CanRead) ReadPixel(sourcePixel);
                    if (target.CanRead) target.ReadPixel(targetPixel);

                    // process the pixel by custom user operation
                    if (isAdvanced)
                    {
                        TransformPixelAdvancedFunction transformAdvancedFunction = (TransformPixelAdvancedFunction)pass;
                        allowWrite = transformAdvancedFunction(lineTask.PassIndex, sourcePixel, targetPixel, this, target);
                    }
                    else // use simplified version with pixel parameters only
                    {
                        TransformPixelFunction transformFunction = (TransformPixelFunction)pass;
                        allowWrite = transformFunction(lineTask.PassIndex, sourcePixel, targetPixel);
                    }

                    // when write is allowed, copies the value back to the row buffer
                    if (target.CanWrite && allowWrite) target.WritePixel(targetPixel);
                }
            };

            // transforms image per pixel
            for (Int32 passIndex = 0; passIndex < passes.Length; passIndex++)
            {
                ProcessInParallel(passIndex, transformPerPixel, path, parallelTaskCount);
            }
        }

        public void TransformPerPixel(ImageBuffer target, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelFunction[] passes)
        {
            TransformPerPixelBase(target, path, parallelTaskCount, passes);
        }

        public void TransformPerPixelAdvanced(ImageBuffer target, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelAdvancedFunction[] passes)
        {
            TransformPerPixelBase(target, path, parallelTaskCount, passes);
        }

        public void TransformPerPixel(PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(targetFormat, "targetFormat");

            // creates a target bitmap in an appropriate format
            targetImage = new Bitmap(Width, Height, targetFormat);

            // sets image palette if needed
            if (targetFormat.IsIndexed()) targetImage.SetPalette(palette);

            // wraps target image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.WriteOnly))
            {
                TransformPerPixelBase(target, path, parallelTaskCount, passes);
            }
        }

        public void TransformPerPixelAdvanced(PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelAdvancedFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(targetFormat, "targetFormat");

            // creates a target bitmap in an appropriate format
            targetImage = new Bitmap(Width, Height, targetFormat);

            // sets image palette if needed
            if (targetFormat.IsIndexed()) targetImage.SetPalette(palette);

            // wraps target image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.WriteOnly))
            {
                TransformPerPixelBase(target, path, parallelTaskCount, passes);
            }
        }

        public static void TransformImagePerPixel(ImageBuffer source, ImageBuffer target, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelFunction[] passes)
        {
            source.TransformPerPixel(target, path, parallelTaskCount, passes);
        }

        public static void TransformImagePerPixelAdvanced(ImageBuffer source, ImageBuffer target, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelAdvancedFunction[] passes)
        {
            source.TransformPerPixelAdvanced(target, path, parallelTaskCount, passes);
        }

        public static void TransformImagePerPixel(ImageBuffer source, PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelFunction[] passes)
        {
            source.TransformPerPixel(targetFormat, palette, out targetImage, path, parallelTaskCount, passes);
        }

        public static void TransformImagePerPixelAdvanced(ImageBuffer source, PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelAdvancedFunction[] passes)
        {
            source.TransformPerPixelAdvanced(targetFormat, palette, out targetImage, path, parallelTaskCount, passes);
        }

        public static void TransformImagePerPixel(Image sourceImage, PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadWrite))
            {
                source.TransformPerPixel(targetFormat, palette, out targetImage, path, parallelTaskCount, passes);
            }
        }

        public static void TransformImagePerPixelAdvanced(Image sourceImage, PixelFormat targetFormat, List<Color> palette, out  Image targetImage, IList<Point> path = null, Int32 parallelTaskCount = 4, params TransformPixelAdvancedFunction[] passes)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadWrite))
            {
                source.TransformPerPixelAdvanced(targetFormat, palette, out targetImage, path, parallelTaskCount, passes);
            }
        }

        #endregion

        #region | Scan colors methods |

        public void ScanColors(IColorQuantizer quantizer, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(quantizer, "quantizer");

            // determines which method of color retrieval to use
            IList<Point> path = quantizer.GetPointPath(Width, Height);

            // use different scanning method depending whether the image format is indexed
            ProcessPixelFunction scanColors = (passIndex, pixel) =>
            {
                Color color = pixel.GetColor();
                quantizer.AddColor(color, pixel.X, pixel.Y);
                return false;
            };

            // performs the image scan, using a chosen method
            ProcessPerPixel(path, parallelTaskCount, scanColors);
        }

        public static void ScanImageColors(Image sourceImage, IColorQuantizer quantizer, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                source.ScanColors(quantizer, parallelTaskCount);
            }
        }

        #endregion

        #region | Synthetize palette methods |

        public List<Color> SynthetizePalette(IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(quantizer, "quantizer");

            // prepares quantizer for another round
            quantizer.Prepare(bitmap);

            // scans the source image for the colors
            ScanColors(quantizer, parallelTaskCount);

            // synthetises the palette, and returns the result
            return quantizer.GetPalette(colorCount);
        }

        public static List<Color> SynthetizeImagePalette(Image sourceImage, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                return source.SynthetizePalette(quantizer, colorCount, parallelTaskCount);
            }
        }

        #endregion

        #region | Quantize methods |

        public void Quantize(ImageBuffer target, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // performs the pure quantization wihout dithering
            Quantize(target, quantizer, null, colorCount, parallelTaskCount);
        }

        public void Quantize(ImageBuffer target, IColorQuantizer quantizer, IColorDitherer ditherer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(target, "target");
            Guard.CheckNull(quantizer, "quantizer");

            // initializes quantization parameters
            Boolean isTargetIndexed = target.PixelFormat.IsIndexed();

            // prepares the palette if needed
            List<Color> targetPalette = isTargetIndexed ? SynthetizePalette(quantizer, colorCount, parallelTaskCount) : null;

            // updates the bitmap palette
            target.bitmap.SetPalette(targetPalette);
            target.UpdatePalette(true);

            // prepares ditherer (optional)
            if (ditherer != null) ditherer.Prepare(quantizer, colorCount, this, target);

            // prepares the quantization function
            TransformPixelFunction quantize = (passIndex, sourcePixel, targetPixel) =>
            {
                // reads the pixel color
                Color color = sourcePixel.GetColor();

                // converts alpha to solid color
                color = QuantizationHelper.ConvertAlpha(color);

                // quantizes the pixel
                targetPixel.SetColor(color, quantizer);

                // marks pixel as processed by default
                Boolean result = true;

                // preforms inplace dithering (optional)
                if (ditherer != null && ditherer.IsInplace)
                {
                    result = ditherer.ProcessPixel(passIndex, sourcePixel, targetPixel);
                }

                // returns the result
                return result;
            };

            // generates the target image
            IList<Point> path = quantizer.GetPointPath(Width, Height);
            TransformPerPixelBase(target, path, parallelTaskCount, quantize);

            // preforms non-inplace dithering (optional)
            if (ditherer != null && !ditherer.IsInplace)
            {
                Dither(target, ditherer, quantizer, colorCount, 1);
            }

            // finishes the dithering (optional)
            if (ditherer != null) ditherer.Finish();

            // cleans up the quantizer
            quantizer.Finish();
        }

        public static Image QuantizeImage(ImageBuffer source, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // performs the pure quantization wihout dithering
            return QuantizeImage(source, quantizer, null, colorCount, parallelTaskCount);
        }

        public static Image QuantizeImage(ImageBuffer source, IColorQuantizer quantizer, IColorDitherer ditherer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");

            // creates a target bitmap in an appropriate format
            PixelFormat targetPixelFormat = Extend.GetFormatByColorCount(colorCount);
            Image result = new Bitmap(source.Width, source.Height, targetPixelFormat);

            // lock mode
            ImageLockMode lockMode = ditherer == null ? ImageLockMode.WriteOnly : ImageLockMode.ReadWrite;

            // wraps target image to a buffer
            using (ImageBuffer target = new ImageBuffer(result, lockMode))
            {
                source.Quantize(target, quantizer, ditherer, colorCount, parallelTaskCount);
                return result;
            }
        }

        public static Image QuantizeImage(Image sourceImage, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // performs the pure quantization wihout dithering
            return QuantizeImage(sourceImage, quantizer, null, colorCount, parallelTaskCount);
        }

        public static Image QuantizeImage(Image sourceImage, IColorQuantizer quantizer, IColorDitherer ditherer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // lock mode
            ImageLockMode lockMode = ditherer == null ? ImageLockMode.ReadOnly : ImageLockMode.ReadWrite;

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, lockMode))
            {
                return QuantizeImage(source, quantizer, ditherer, colorCount, parallelTaskCount);
            }
        }

        #endregion

        #region | Calculate mean error methods |

        public Double CalculateMeanError(ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(target, "target");

            // initializes the error
            Int64 totalError = 0;

            // prepares the function
            TransformPixelFunction calculateMeanError = (passIndex, sourcePixel, targetPixel) =>
            {
                Color sourceColor = sourcePixel.GetColor();
                Color targetColor = targetPixel.GetColor();
                totalError += ColorModelHelper.GetColorEuclideanDistance(ColorModel.RedGreenBlue, sourceColor, targetColor);
                return false;
            };

            // performs the image scan, using a chosen method
            IList<Point> standardPath = new StandardPathProvider().GetPointPath(Width, Height);
            TransformPerPixelBase(target, standardPath, parallelTaskCount, calculateMeanError);

            // returns the calculates RMSD
            return Math.Sqrt(totalError / (3.0 * Width * Height));
        }

        public static Double CalculateImageMeanError(ImageBuffer source, ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");

            // use other override to calculate error
            return source.CalculateMeanError(target, parallelTaskCount);
        }

        public static Double CalculateImageMeanError(ImageBuffer source, Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateMeanError(target, parallelTaskCount);
            }
        }

        public static Double CalculateImageMeanError(Image sourceImage, ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateMeanError(target, parallelTaskCount);
            }
        }

        public static Double CalculateImageMeanError(Image sourceImage, Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateMeanError(target, parallelTaskCount);
            }
        }

        #endregion

        #region | Calculate normalized mean error methods |

        public Double CalculateNormalizedMeanError(ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            return CalculateMeanError(target, parallelTaskCount) / 255.0;
        }

        public static Double CalculateImageNormalizedMeanError(ImageBuffer source, Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateNormalizedMeanError(target, parallelTaskCount);
            }
        }

        public static Double CalculateImageNormalizedMeanError(Image sourceImage, ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateNormalizedMeanError(target, parallelTaskCount);
            }
        }

        public static Double CalculateImageNormalizedMeanError(ImageBuffer source, ImageBuffer target, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");

            // use other override to calculate error
            return source.CalculateNormalizedMeanError(target, parallelTaskCount);
        }

        public static Double CalculateImageNormalizedMeanError(Image sourceImage, Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                return source.CalculateNormalizedMeanError(target, parallelTaskCount);
            }
        }

        #endregion

        #region | Change pixel format methods |

        public void ChangeFormat(ImageBuffer target, IColorQuantizer quantizer, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(target, "target");
            Guard.CheckNull(quantizer, "quantizer");

            // gathers some information about the target format
            Boolean hasSourceAlpha = PixelFormat.HasAlpha();
            Boolean hasTargetAlpha = target.PixelFormat.HasAlpha();
            Boolean isTargetIndexed = target.PixelFormat.IsIndexed();
            Boolean isSourceDeepColor = PixelFormat.IsDeepColor();
            Boolean isTargetDeepColor = target.PixelFormat.IsDeepColor();

            // prepares the palette if needed
            if (isTargetIndexed)
            {
                // synthetises palette using provided quantizer
                List<Color> targetPalette = SynthetizePalette(quantizer, target.PixelFormat.GetColorCount(), parallelTaskCount);

                // updates the bitmap palette
                target.bitmap.SetPalette(targetPalette);
                target.UpdatePalette(true);
            }

            // prepares the quantization function
            TransformPixelFunction changeFormat = (passIndex, sourcePixel, targetPixel) =>
            {
                // if both source and target formats are deep color formats, copies a value directly
                if (isSourceDeepColor && isTargetDeepColor)
                {
                    //UInt64 value = sourcePixel.Value;
                    //targetPixel.SetValue(value);
                }
                else
                {
                    // retrieves a source image color
                    Color color = sourcePixel.GetColor();

                    // if alpha is not present in the source image, but is present in the target, make one up
                    if (!hasSourceAlpha && hasTargetAlpha)
                    {
                        Int32 argb = 255 << 24 | color.R << 16 | color.G << 8 | color.B;
                        color = Color.FromArgb(argb);
                    }

                    // sets the color to a target pixel
                    targetPixel.SetColor(color, quantizer);
                }

                // allows to write (obviously) the transformed pixel
                return true;
            };

            // generates the target image
            IList<Point> standardPath = new StandardPathProvider().GetPointPath(Width, Height);
            TransformPerPixelBase(target, standardPath, parallelTaskCount, changeFormat);
        }

        public static void ChangeFormat(ImageBuffer source, PixelFormat targetFormat, IColorQuantizer quantizer, out Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");

            // creates a target bitmap in an appropriate format
            targetImage = new Bitmap(source.Width, source.Height, targetFormat);

            // wraps target image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.WriteOnly))
            {
                source.ChangeFormat(target, quantizer, parallelTaskCount);
            }
        }

        public static void ChangeFormat(Image sourceImage, PixelFormat targetFormat, IColorQuantizer quantizer, out Image targetImage, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                ChangeFormat(source, targetFormat, quantizer, out targetImage, parallelTaskCount);
            }
        }

        #endregion

        #region | Dithering methods |

        public void Dither(ImageBuffer target, IColorDitherer ditherer, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(target, "target");
            Guard.CheckNull(ditherer, "ditherer");
            Guard.CheckNull(quantizer, "quantizer");

            // prepares ditherer for another round
            ditherer.Prepare(quantizer, colorCount, this, target);

            // processes the image via the ditherer
            IList<Point> path = ditherer.GetPointPath(Width, Height);
            TransformPerPixel(target, path, parallelTaskCount, ditherer.ProcessPixel);
        }

        public static void DitherImage(ImageBuffer source, ImageBuffer target, IColorDitherer ditherer, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");

            // use other override to calculate error
            source.Dither(target, ditherer, quantizer, colorCount, parallelTaskCount);
        }

        public static void DitherImage(ImageBuffer source, Image targetImage, IColorDitherer ditherer, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(source, "source");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                source.Dither(target, ditherer, quantizer, colorCount, parallelTaskCount);
            }
        }

        public static void DitherImage(Image sourceImage, ImageBuffer target, IColorDitherer ditherer, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                source.Dither(target, ditherer, quantizer, colorCount, parallelTaskCount);
            }
        }

        public static void DitherImage(Image sourceImage, Image targetImage, IColorDitherer ditherer, IColorQuantizer quantizer, Int32 colorCount, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");
            Guard.CheckNull(targetImage, "targetImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            using (ImageBuffer target = new ImageBuffer(targetImage, ImageLockMode.ReadOnly))
            {
                // use other override to calculate error
                source.Dither(target, ditherer, quantizer, colorCount, parallelTaskCount);
            }
        }

        #endregion

        #region | Gamma correction |

        public void CorrectGamma(Single gamma, IColorQuantizer quantizer, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(quantizer, "quantizer");

            // determines which method of color retrieval to use
            IList<Point> path = quantizer.GetPointPath(Width, Height);

            // calculates gamma ramp
            Int32[] gammaRamp = new Int32[256];

            for (Int32 index = 0; index < 256; ++index)
            {
                gammaRamp[index] = Clamp((Int32)((255.0f * Math.Pow(index / 255.0f, 1.0f / gamma)) + 0.5f));
            }

            // use different scanning method depending whether the image format is indexed
            ProcessPixelFunction correctGamma = (passIndex, pixel) =>
            {
                Color oldColor = pixel.GetColor();
                Int32 red = gammaRamp[oldColor.R];
                Int32 green = gammaRamp[oldColor.G];
                Int32 blue = gammaRamp[oldColor.B];
                Color newColor = Color.FromArgb(red, green, blue);
                pixel.SetColor(newColor, quantizer);
                return true;
            };

            // performs the image scan, using a chosen method
            ProcessPerPixel(path, parallelTaskCount, correctGamma);
        }

        public static void CorrectImageGamma(Image sourceImage, Single gamma, IColorQuantizer quantizer, Int32 parallelTaskCount = 4)
        {
            // checks parameters
            Guard.CheckNull(sourceImage, "sourceImage");

            // wraps source image to a buffer
            using (ImageBuffer source = new ImageBuffer(sourceImage, ImageLockMode.ReadOnly))
            {
                source.CorrectGamma(gamma, quantizer, parallelTaskCount);
            }
        }

        #endregion

        #region | Palette methods |

        public static Int32 Clamp(Int32 value, Int32 minimum = 0, Int32 maximum = 255)
        {
            if (value < minimum) value = minimum;
            if (value > maximum) value = maximum;
            return value;
        }

        private List<Color> UpdatePalette(Boolean forceUpdate = false)
        {
            if (IsIndexed && (cachedPalette == null || forceUpdate))
            {
                cachedPalette = bitmap.GetPalette();
            }

            return cachedPalette;
        }

        public Color GetPaletteColor(Int32 paletteIndex)
        {
            return cachedPalette[paletteIndex];
        }

        #endregion

        #region << IDisposable >>

        public void Dispose()
        {
            // releases the image lock
            lock (bitmap) bitmap.UnlockBits(bitmapData);
        }

        #endregion

        #region | Sub-classes |

        private class LineTask
        {
            /// <summary>
            /// Get or sets the pass index.
            /// </summary>
            public Int32 PassIndex { get; private set; }

            /// <summary>
            /// Gets or sets the start offset.
            /// </summary>
            public Int32 StartOffset { get; private set; }

            /// <summary>
            /// Gets or sets the end offset.
            /// </summary>
            public Int32 EndOffset { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="LineTask"/> class.
            /// </summary>
            public LineTask(Int32 passIndex, Int32 startOffset, Int32 endOffset)
            {
                PassIndex = passIndex;
                StartOffset = startOffset;
                EndOffset = endOffset;
            }
        }

        #endregion
    }
    /// <summary>
    /// This is a pixel format independent pixel.
    /// </summary>
    public class Pixel : IDisposable
    {
        #region | Constants |

        internal const Byte Zero = 0;
        internal const Byte One = 1;
        internal const Byte Two = 2;
        internal const Byte Four = 4;
        internal const Byte Eight = 8;

        internal const Byte NibbleMask = 0xF;
        internal const Byte ByteMask = 0xFF;

        internal const Int32 AlphaShift = 24;
        internal const Int32 RedShift = 16;
        internal const Int32 GreenShift = 8;
        internal const Int32 BlueShift = 0;

        internal const Int32 AlphaMask = ByteMask << AlphaShift;
        internal const Int32 RedGreenBlueMask = 0xFFFFFF;
        internal const Single HighColorFactor = 8192.0f / 256.0f;

        #endregion

        #region | Fields |

        private Type pixelType;
        private Int32 bitOffset;
        private Object pixelData;
        private IntPtr pixelDataPointer;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets the X.
        /// </summary>
        public Int32 X { get; private set; }

        /// <summary>
        /// Gets the Y.
        /// </summary>
        public Int32 Y { get; private set; }

        /// <summary>
        /// Gets the parent buffer.
        /// </summary>
        public ImageBuffer Parent { get; private set; }

        #endregion

        #region | Calculated properties |

        /// <summary>
        /// Gets a value indicating whether this instance is indexed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is indexed; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsIndexed
        {
            get { return Parent.IsIndexed; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct.
        /// </summary>
        public Pixel(ImageBuffer parent)
        {
            Parent = parent;

            Initialize();
        }

        private void Initialize()
        {
            // creates pixel data
            pixelType = IsIndexed ? GetIndexedType(Parent.PixelFormat) : GetNonIndexedType(Parent.PixelFormat);
            NewExpression newType = Expression.New(pixelType);
            UnaryExpression convertNewType = Expression.Convert(newType, typeof(Object));
            Expression<Func<Object>> indexedExpression = Expression.Lambda<Func<Object>>(convertNewType);
            pixelData = indexedExpression.Compile().Invoke();
            pixelDataPointer = MarshalToPointer(pixelData);
        }

        #endregion

        #region | Read methods |

        public Int32 GetIndex()
        {
            Int32 result;

            // determines whether the format is indexed
            if (IsIndexed)
            {
                result = ((IIndexedPixel)pixelData).GetIndex(bitOffset);
            }
            else // not possible to get index from a non-indexed format
            {
                String message = string.Format("Cannot retrieve index for a non-indexed format. Please use Color (or Value) property instead.");
                throw new NotSupportedException(message);
            }

            return result;
        }

        public Color GetColor()
        {
            Color result;

            // retrieves color by the index
            if (IsIndexed)
            {
                Int32 index = ((IIndexedPixel)pixelData).GetIndex(bitOffset);
                result = Parent.GetPaletteColor(index);
            }
            else // retrieves color directly
            {
                result = ((INonIndexedPixel)pixelData).GetColor();
            }

            // returns the color
            return result;
        }

        #endregion

        #region | Write methods |

        public void SetIndex(Int32 index, Byte[] buffer = null)
        {
            // determines whether the format is indexed
            if (IsIndexed)
            {
                ((IIndexedPixel)pixelData).SetIndex(bitOffset, (Byte)index);
            }
            else // cannot write color to an indexed format
            {
                String message = string.Format("Cannot set index for a non-indexed format. Please use Color (or Value) property instead.");
                throw new NotSupportedException(message);
            }
        }

        public void SetColor(Color color, IColorQuantizer quantizer)
        {
            // determines whether the format is indexed
            if (IsIndexed)
            {
                // last chance if quantizer is provided, use it
                if (quantizer != null)
                {
                    Byte index = (Byte)quantizer.GetPaletteIndex(color, X, Y);
                    ((IIndexedPixel)pixelData).SetIndex(bitOffset, index);
                }
                else // cannot write color to an index format
                {
                    String message = string.Format("Cannot retrieve color for an indexed format. Use GetPixelIndex() instead.");
                    throw new NotSupportedException(message);
                }
            }
            else // sets color to a non-indexed format
            {
                ((INonIndexedPixel)pixelData).SetColor(color);
            }
        }

        #endregion

        #region | Helper methods |

        /// <summary>
        /// Gets the type of the indexed pixel format.
        /// </summary>
        internal Type GetIndexedType(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed: return typeof(PixelData1Indexed);
                case PixelFormat.Format4bppIndexed: return typeof(PixelData4Indexed);
                case PixelFormat.Format8bppIndexed: return typeof(PixelData8Indexed);

                default:
                    String message = String.Format("This pixel format '{0}' is either non-indexed, or not supported.", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the type of the non-indexed pixel format.
        /// </summary>
        internal Type GetNonIndexedType(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppArgb1555: return typeof(PixelDataArgb1555);
                case PixelFormat.Format16bppGrayScale: return typeof(PixelDataGray16);
                case PixelFormat.Format16bppRgb555: return typeof(PixelDataRgb555);
                case PixelFormat.Format16bppRgb565: return typeof(PixelDataRgb565);
                case PixelFormat.Format24bppRgb: return typeof(PixelDataRgb888);
                case PixelFormat.Format32bppRgb: return typeof(PixelDataRgb8888);
                case PixelFormat.Format32bppArgb: return typeof(PixelDataArgb8888);
                // case PixelFormat.Format32bppPArgb: return typeof (PixelDataPArgb8888);
                case PixelFormat.Format48bppRgb: return typeof(PixelDataRgb48);
                case PixelFormat.Format64bppArgb: return typeof(PixelDataArgb64);
                // case PixelFormat.Format64bppPArgb: return typeof (PixelDataPArgb64);

                default:
                    String message = String.Format("This pixel format '{0}' is either indexed, or not supported.", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        private static IntPtr MarshalToPointer(Object data)
        {
            Int32 size = Marshal.SizeOf(data);
            IntPtr pointer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, pointer, false);
            return pointer;
        }

        #endregion

        #region | Update methods |

        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public void Update(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
            bitOffset = Parent.GetBitOffset(x);
        }

        /// <summary>
        /// Reads the raw data.
        /// </summary>
        /// <param name="imagePointer">The image pointer.</param>
        public void ReadRawData(IntPtr imagePointer)
        {
            pixelData = Marshal.PtrToStructure(imagePointer, pixelType);
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        public void ReadData(Byte[] buffer, Int32 offset)
        {
            Marshal.Copy(buffer, offset, pixelDataPointer, Parent.BytesPerPixel);
            pixelData = Marshal.PtrToStructure(pixelDataPointer, pixelType);
        }

        /// <summary>
        /// Writes the raw data.
        /// </summary>
        /// <param name="imagePointer">The image pointer.</param>
        public void WriteRawData(IntPtr imagePointer)
        {
            Marshal.StructureToPtr(pixelData, imagePointer, false);
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        public void WriteData(Byte[] buffer, Int32 offset)
        {
            Marshal.Copy(pixelDataPointer, buffer, offset, Parent.BytesPerPixel);
        }

        #endregion

        #region << IDisposable >>

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Marshal.FreeHGlobal(this.pixelDataPointer);
        }


        #endregion
    }
    public class QuantizationHelper
    {
        private const int Alpha = 255 << 24;
        private static readonly Color BackgroundColor;
        private static readonly Double[] Factors;

        static QuantizationHelper()
        {
            BackgroundColor = SystemColors.Control;
            Factors = PrecalculateFactors();
        }

        /// <summary>
        /// Precalculates the alpha-fix values for all the possible alpha values (0-255).
        /// </summary>
        private static Double[] PrecalculateFactors()
        {
            Double[] result = new Double[256];

            for (Int32 value = 0; value < 256; value++)
            {
                result[value] = value / 255.0;
            }

            return result;
        }

        /// <summary>
        /// Converts the alpha blended color to a non-alpha blended color.
        /// </summary>
        /// <param name="color">The alpha blended color (ARGB).</param>
        /// <returns>The non-alpha blended color (RGB).</returns>
        internal static Color ConvertAlpha(Color color)
        {
            Int32 argb;
            return ConvertAlpha(color, out argb);
        }

        /// <summary>
        /// Converts the alpha blended color to a non-alpha blended color.
        /// </summary>
        internal static Color ConvertAlpha(Color color, out Int32 argb)
        {
            Color result = color;

            if (color.A < 255)
            {
                // performs a alpha blending (second color is BackgroundColor, by default a Control color)
                Double colorFactor = Factors[color.A];
                Double backgroundFactor = Factors[255 - color.A];
                Int32 red = (Int32)(color.R * colorFactor + BackgroundColor.R * backgroundFactor);
                Int32 green = (Int32)(color.G * colorFactor + BackgroundColor.G * backgroundFactor);
                Int32 blue = (Int32)(color.B * colorFactor + BackgroundColor.B * backgroundFactor);
                argb = red << 16 | green << 8 | blue;
                Color.FromArgb(red, green, blue);
                result = Color.FromArgb(Alpha | argb);
            }
            else
            {
                argb = color.R << 16 | color.G << 8 | color.B;
            }

            return result;
        }
    }


    #endregion


    //
    /// <summary>
    /// Bitmap. easy Pixel Format Conversion .
    /// </summary>
    public static class SimpleHelper
    {

        public static Color GetGrayColor(Color color__1)
        {
            Int32 gray = (Int32)(0.3f * color__1.R + 0.52f * color__1.G + 0.11f * color__1.B);
            return Color.FromArgb(color__1.A, gray, gray, gray);
        }

        public static BaseColorCacheQuantizer InitializeNewQuantizer()
        {
            return new DistinctSelectionQuantizer();
        }
        public static BaseColorCache GetNewCacheProvider(bool UsePreciseMethod = true)
        {
            return UsePreciseMethod ? (BaseColorCache)new EuclideanDistanceColorCache() : new OctreeColorCache();
        }

        //
        public static Bitmap ConvertBitmap(this Bitmap SourceImage, PixelFormat TargePixelFormat_, bool UseGrayColor_ = false, bool UsePreciseMethod = true)
        {
            BaseColorCacheQuantizer Quantizer = InitializeNewQuantizer();
            BaseColorCache colorCache = GetNewCacheProvider(UsePreciseMethod);

            Quantizer.ChangeCacheProvider(colorCache);
            return ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, UseGrayColor_);
        }
        public static Bitmap ConvertBitmap(this Bitmap SourceImage, PixelFormat TargePixelFormat_, Func<Color, Color> CustomGrayColor_, bool UsePreciseMethod = true)
        {
            BaseColorCacheQuantizer Quantizer = InitializeNewQuantizer();
            BaseColorCache colorCache = GetNewCacheProvider(UsePreciseMethod);

            Quantizer.ChangeCacheProvider(colorCache);
            return ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, CustomGrayColor_);
        }

        public static Bitmap ConvertBitmap(this Bitmap SourceImage, PixelFormat TargePixelFormat_, BaseColorCacheQuantizer Quantizer, bool UseGrayColor_ = false)
        {
            //Return ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, CType(If(UseGrayColor_, AddressOf GetGrayColor, Nothing), Func(Of Color, Color)))
            if (UseGrayColor_)
            {
                return ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, GetGrayColor);
            }
            else
            {
                return ConvertBitmap(SourceImage, TargePixelFormat_, Quantizer, (Func<Color, Color>)null);
            }

        }
        public static Bitmap ConvertBitmap(this Bitmap SourceImage, PixelFormat TargePixelFormat_, BaseColorCacheQuantizer Quantizer, Func<Color, Color> CustomGrayColor_)
        {
            if (Quantizer == null)
                Quantizer = InitializeNewQuantizer();
            //
            List<Color> palette = null;
            Image targetImage = null;
            // indexed formats require 2 passes - one more pass to determines colors for palette beforehand
            if (TargePixelFormat_.IsIndexed())
            {
                Quantizer.Prepare(SourceImage);
                // Pass: scan grayscale
                ImageBuffer.ProcessPerPixel(SourceImage, null, 4, (passIndex, pixel) =>
                {
                    Color color_;
                    color_ = pixel.GetColor();
                    if (CustomGrayColor_ != null)
                        color_ = CustomGrayColor_(color_);
                    Quantizer.AddColor(color_, pixel.X, pixel.Y);
                    return true;
                });

                // determines palette
                palette = Quantizer.GetPalette(TargePixelFormat_.GetColorCount());
            }
            // Pass: apply grayscale
            ImageBuffer.TransformImagePerPixel(SourceImage, TargePixelFormat_, palette, out targetImage, null, 4, (passIndex, sourcePixel, targetPixel) =>
            {
                Color color_;
                color_ = sourcePixel.GetColor();
                if (CustomGrayColor_ != null)
                    color_ = CustomGrayColor_(color_);
                targetPixel.SetColor(color_, Quantizer);
                return true;
            });

            return (Bitmap)targetImage;
        }

        public class ImageProcess
        {

            /// <summary>
            /// The source of a bitmap to be converted.
            /// </summary>
            /// <param name="SourceImage__"></param>
            /// <remarks></remarks>
            public ImageProcess(Bitmap SourceImage__)
            {
                SourceImage_ = SourceImage__;
                Quantizer_ = InitializeNewQuantizer();
                UsePreciseMethod = false;
            }

            Bitmap SourceImage_;
            public Bitmap SourceImage
            {
                get { return SourceImage_; }
            }

            BaseColorCacheQuantizer Quantizer_;
            public BaseColorCacheQuantizer Quantizer
            {
                get { return Quantizer_; }
            }
            //Use precise method
            bool UsePreciseMethod_ = false;
            public bool UsePreciseMethod
            {
                get { return UsePreciseMethod_; }
                set
                {
                    UsePreciseMethod_ = value;
                    Quantizer.ChangeCacheProvider(GetNewCacheProvider(UsePreciseMethod_));
                }
            }

            public Bitmap ConvertBitmap(PixelFormat TargePixelFormat_, bool UseGrayColor_ = false)
            {
                return SimpleHelper.ConvertBitmap(SourceImage_, TargePixelFormat_, Quantizer, UseGrayColor_);
            }
            public Bitmap ConvertBitmap(PixelFormat TargePixelFormat_, Func<Color, Color> CustomGrayColor_)
            {
                return SimpleHelper.ConvertBitmap(SourceImage_, TargePixelFormat_, Quantizer, CustomGrayColor_);
            }

        }


    }

}
