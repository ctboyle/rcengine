using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.Utility;

namespace RC.Engine.Rendering
{
    public enum ElementType
    {
        Position,
        BlendWeight,
        Normal,
        Color,
        Texture,
        Fog,
        PointSize,
        BlendIndices,
        Tangent,
        Bitangent
    }

    /// <summary>
    /// Helps create custom vertex declarations based on the configuration of vertex elements.
    /// </summary>
    public class RCVertexAttributes
    {
        public enum ChannelCount
        {
            None,
            One,
            Two,
            Three,
            Four
        }
        
        private class RCVertexElement
        {
            public ElementType ElementType;
            public int Offset;
            public ChannelCount Channels;

        }

        private Dictionary<ElementType, RCVertexElement> _vertexElementDescriptors
            = new Dictionary<ElementType, RCVertexElement>();

        private int m_iChannelQuantity = 0;

        public RCVertexAttributes()
        {
            // Initialize element dictionary to defaults.
#if XBOX360
            foreach(ElementType type in EnumHelper.GetValues<ElementType>())
#else
            foreach (ElementType type in Enum.GetValues(typeof(ElementType)))
#endif
            {
                RCVertexElement element = new RCVertexElement();
                element.ElementType = type;
                element.Offset = -1;
                element.Channels = ChannelCount.None;

                _vertexElementDescriptors[type] = element;
            }
        }

        /// <summary>
        /// Sets the number of channels for each vertex element of that type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="numChannels"></param>
        public void SetElementChannels(ElementType type, ChannelCount numChannels)
        {
            _vertexElementDescriptors[type].Channels = numChannels;
            UpdateOffsets();
        }


        /// <summary>
        /// Gets the number of 'float' channels used by all the attributes.
        /// </summary>
        /// <returns>the number of 'float' channels used by all the attributes.</returns>
        public int GetChannelQuantity()
        {
            return m_iChannelQuantity;
        }

        /// <summary>
        /// Gets the number of channels allocated to an element for the given element type.
        /// </summary>
        /// <param name="type">The vertex element type.</param>
        /// <returns>Number of channels allocated for that type.</returns>
        public ChannelCount GetElementChannels(ElementType type)
        {
            return _vertexElementDescriptors[type].Channels;
        }

        /// <summary>
        /// Gets the offset to the element type for a vertex element.
        /// </summary>
        /// <param name="type">The vertex element type.</param>
        /// <returns>Size in bytes of the element offset</returns>
        public int GetElementOffset(ElementType type)
        {
            return _vertexElementDescriptors[type].Offset;
        }

        /// <summary>
        /// Gets whether the vertex has the associated element.
        /// </summary>
        /// <param name="type">The vertex element type.</param>
        /// <returns>Indicates if the element is in the vertex.</returns>
        public bool HasElement(ElementType type)
        {
            return (_vertexElementDescriptors[type].Channels != ChannelCount.None);
        }

        /// <summary>
        /// Gets a vertex element array for the defined vertex attribute.
        /// </summary>
        /// <returns></returns>
        public VertexElement[] VertexElements()
        {
            List<VertexElement> elements = new List<VertexElement>();

#if XBOX360
            foreach (ElementType type in EnumHelper.GetValues<ElementType>())
#else
            foreach (ElementType type in Enum.GetValues(typeof(ElementType)))
#endif
            {
                RCVertexElement element = _vertexElementDescriptors[type];

                // If element is included with this vertex, create a VertexElement definition for it.
                if (element.Channels != ChannelCount.None)
                {
                    elements.Add(new VertexElement(
                        0,
                        (short)(element.Offset * sizeof(float)),
                        GetVertexFormat(element),
                        VertexElementMethod.Default,
                        GetVertexUsage(element),
                        0));
                }
            }

            return elements.ToArray();

        }

        /// <summary>
        /// Creates a vertex declaration for the defined vertex.
        /// </summary>
        /// <param name="device">Graphics device to create the declaration.</param>
        /// <returns>The vertex declaration</returns>
        public VertexDeclaration CreateVertexDeclaration(GraphicsDevice device)
        {
            return new VertexDeclaration(device, VertexElements());
        }


        static private VertexElementFormat GetVertexFormat(RCVertexElement element)
        {
            VertexElementFormat format;
            switch (element.Channels)
            {
                case ChannelCount.One:
                    format = VertexElementFormat.Single;
                    break;
                case ChannelCount.Two:
                    format = VertexElementFormat.Vector2;
                    break;
                case ChannelCount.Three:
                    format = VertexElementFormat.Vector3;
                    break;
                case ChannelCount.Four:
                    format = VertexElementFormat.Vector4;
                    break;
                case ChannelCount.None:
                default:
                    throw new InvalidOperationException("Invalid channel count.");

            }
            return format;
        }

        static private VertexElementUsage GetVertexUsage(RCVertexElement element)
        {
            VertexElementUsage usage;
            switch (element.ElementType)
            {
                case ElementType.Color:
                    usage = VertexElementUsage.Color;
                    break;
                case ElementType.Position:
                    usage = VertexElementUsage.Position;
                    break;
                case ElementType.Normal:
                    usage = VertexElementUsage.Normal;
                    break;
                case ElementType.Texture:
                    usage = VertexElementUsage.TextureCoordinate;
                    break;
                case ElementType.BlendWeight:
                    usage = VertexElementUsage.BlendWeight;
                    break;
                case ElementType.Fog:
                    usage = VertexElementUsage.Fog;
                    break;
                case ElementType.PointSize:
                    usage = VertexElementUsage.PointSize;
                    break;
                case ElementType.Tangent:
                    usage = VertexElementUsage.Tangent;
                    break;
                case ElementType.Bitangent:
                    usage = VertexElementUsage.Binormal;
                    break;
                case ElementType.BlendIndices:
                    usage = VertexElementUsage.BlendIndices;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return usage;
        }

        // Support for comparing vertex buffer attributes with vertex program
        // input attributes.

        //// public static bool operator== ( RCAttributes leftAttr, RCAttributes rightAttr);
        //// public static bool operator== ( RCAttributes leftAttr, RCAttributes rightAttr);


        private void UpdateOffsets()
        {
            m_iChannelQuantity = 0;

            // Compute offsets for each element type
#if XBOX360
            foreach (ElementType type in EnumHelper.GetValues<ElementType>())
#else
            foreach (ElementType type in Enum.GetValues(typeof(ElementType)))
#endif
            {
                RCVertexElement element = _vertexElementDescriptors[type];
                
                // Check and see if element is included in this buffer
                if (element.Channels != ChannelCount.None)
                {                    
                    element.Offset = m_iChannelQuantity;
                    m_iChannelQuantity += (int)element.Channels;
                }
            }
        }
    }
}
