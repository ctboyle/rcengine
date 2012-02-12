using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.ContentManagement;

namespace RC.Engine.Rendering
{
    public class RCMemoryDefinedGeometry : RCDeviceResource
    {

        private RCVertexRefrence _vertexRefrence;

        private RCVertexBuffer _vBuffer;
        private RCIndexBuffer _iBuffer;


        public RCVertexBuffer RCVertexBuffer
        {
            get { return _vBuffer; }
        }

        public RCIndexBuffer RCIndexBuffer
        {
            get { return _iBuffer; }
        }


        public RCVertexRefrence GetVertexRefrence(
            int startIndex,
            int baseVertx,
            int numVertices,
            int numPrim)
        {
            return new RCVertexRefrence(
                Graphics,
                _iBuffer,
                _vBuffer,
                numVertices,
                baseVertx,
                startIndex,
                numPrim);

        }

        public RCMemoryDefinedGeometry(
            RCIndexBuffer indexBuffer,
            RCVertexBuffer vertexBuffer)
        {
            _iBuffer = indexBuffer;
            _vBuffer = vertexBuffer;
        }

        protected override bool IsOnDevice
        {
            get { return _iBuffer.IndexBuffer != null; }
        }

        protected override void  SetOnDevice()
        {
            _vBuffer.Enable(Graphics);
            _iBuffer.Enable(Graphics);
        }
        protected override void RemoveFromDevice()
        {
            _iBuffer.Disable();
            _vBuffer.Disable();
        }       

    }
}
