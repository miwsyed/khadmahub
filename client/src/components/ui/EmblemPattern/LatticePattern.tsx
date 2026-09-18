/**
 * A restrained eight-point-star lattice, the geometric vocabulary common to
 * civic architecture across the region. Rendered as a tiling SVG pattern so
 * it reads as texture, not decoration competing with the form.
 */
export function LatticePattern() {
  return (
    <svg
      className="lattice-pattern"
      width="100%"
      height="100%"
      xmlns="http://www.w3.org/2000/svg"
      aria-hidden="true"
    >
      <defs>
        <pattern
          id="khadmahub-lattice"
          x="0"
          y="0"
          width="64"
          height="64"
          patternUnits="userSpaceOnUse"
        >
          <path
            d="M32 4 L44 16 L60 16 L60 32 L44 44 L32 60 L20 44 L4 44 L4 32 L20 16 Z"
            fill="none"
            stroke="rgba(216,180,106,0.18)"
            strokeWidth="1"
          />
          <circle cx="32" cy="32" r="3" fill="rgba(216,180,106,0.16)" />
        </pattern>
      </defs>
      <rect width="100%" height="100%" fill="url(#khadmahub-lattice)" />
    </svg>
  );
}
