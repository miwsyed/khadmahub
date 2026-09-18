/**
 * A simple original seal mark: a river channel through two banks,
 * ringed like an official civic stamp. Stands in for a municipal crest
 * until Basra's directorate supplies an official emblem file.
 */
import type { SealMarkProps } from "./types/sealMark.types";

export function SealMark({ size = 44, tone = "light" }: SealMarkProps) {
  const ring = tone === "light" ? "#d8b46a" : "#0d3733";
  const fill = tone === "light" ? "#faf8f2" : "#0d3733";
  const wave = tone === "light" ? "#0d3733" : "#d8b46a";

  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 48 48"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      role="img"
      aria-label="KhadmaHub seal"
    >
      <circle cx="24" cy="24" r="22.5" stroke={ring} strokeWidth="1.5" />
      <circle cx="24" cy="24" r="18.5" stroke={ring} strokeWidth="1" opacity="0.6" />
      <circle cx="24" cy="24" r="16" fill={fill} />
      <path
        d="M9 26c3-3 6-3 9 0s6 3 9 0 6-3 9 0"
        stroke={wave}
        strokeWidth="2"
        strokeLinecap="round"
        fill="none"
      />
      <path
        d="M9 20c3-3 6-3 9 0s6 3 9 0 6-3 9 0"
        stroke={wave}
        strokeWidth="1.4"
        strokeLinecap="round"
        fill="none"
        opacity="0.55"
      />
      <path d="M24 8.5v6" stroke={wave} strokeWidth="1.4" strokeLinecap="round" />
    </svg>
  );
}
