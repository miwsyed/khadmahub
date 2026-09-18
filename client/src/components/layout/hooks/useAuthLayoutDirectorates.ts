import { useTranslation } from "react-i18next";

export function useAuthLayoutDirectorates() {
  const { t } = useTranslation();

  return [
    t("directors.civilStatus"),
    t("directors.municipality"),
    t("directors.traffic"),
    t("directors.realEstate"),
  ];
}
