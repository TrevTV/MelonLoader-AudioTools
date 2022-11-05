using System;

namespace NVorbis
{
    /// <summary>
    /// Old interface, current version moved to Contracts.IPacketProvider
    /// </summary>
    [Obsolete("Moved to NVorbis.Contracts.IPacketProvider", true)]
    public interface IPacketProvider : Contracts.IPacketProvider
    {
        /// <summary>
        /// Gets the number of bits of overhead in this stream's container.
        /// </summary>
        [Obsolete("Moved to per-stream IStreamStats instance on IStreamDecoder.Stats or VorbisReader.Stats.", true)]
        long ContainerBits { get; }

        /// <summary>
        /// Retrieves the total number of pages (or frames) this stream uses.
        /// </summary>
        [Obsolete("No longer supported.", true)]
        int GetTotalPageCount();

        /// <summary>
        /// Occurs when the stream is about to change parameters.
        /// </summary>
        [Obsolete("No longer supported.", true)]
        event EventHandler ParameterChange;
    }
}
