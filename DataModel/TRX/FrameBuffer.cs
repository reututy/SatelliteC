using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class FrameBuffer
    {
        private Queue<Frame> frames;
        private int MAX_FRAMES { get; }

        public FrameBuffer(int maxFrames)
        {
            MAX_FRAMES = maxFrames;
        }

        internal int addFrame(Frame frame)
        {
            if (frames.Count + 1 > MAX_FRAMES)
            {
                return Constants.E_MEM_ALLOC;
            }
            frames.Enqueue(frame);
            return Constants.E_NO_SS_ERR;
        }

        internal int getAvailbleSpace()
        {
            return MAX_FRAMES - frames.Count;
        }

        internal Frame removeFrame()
        {
            return frames.Dequeue();
        }

        internal int getFrameCount()
        {
            return frames.Count;
        }
    }
}
