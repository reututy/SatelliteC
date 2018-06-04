using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class FrameBuffer
    {
        int counter;
        public ObservableQueue<Frame> frames;
        private int MAX_FRAMES { get; }

        public FrameBuffer(int maxFrames)
        {
            frames = new ObservableQueue<Frame>();
            MAX_FRAMES = maxFrames;
        }

        internal int addFrame(Frame frame)
        {
            frame.FrameId = counter;
            counter++;
            if (frames.count() + 1 > MAX_FRAMES)
            {
                return Constants.E_MEM_ALLOC;
            }
            frames.enqueue(frame);
            return Constants.E_NO_SS_ERR;
        }

        internal int getAvailbleSpace()
        {
            return MAX_FRAMES - frames.count();
        }

        internal Frame removeFrame()
        {
            return frames.dequeue();
        }

        internal int getFrameCount()
        {
            return frames.count();
        }

        public void clear()
        {
            frames.clear();
        }
    }
}
