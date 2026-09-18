import { useTranslation } from "react-i18next";
import { useToast } from "@/context/ToastContext";
import { useServiceCardContent } from "@/features/dashboard/hooks/useServiceCardContent";
import type { ServiceCardProps } from "./types/serviceCard.types";
import styles from "./ServiceCard.module.css";

export function ServiceCard({ service }: ServiceCardProps) {
  const { notify } = useToast();
  const { t } = useTranslation();
  const { serviceName, serviceDirectorate, serviceDescription, serviceProcessingTime } =
    useServiceCardContent(service);

  const handleStart = () => {
    notify({
      variant: "info",
      title: t("dashboard.card.soonTitle"),
      description: t("dashboard.card.soonDescription", { name: serviceName }),
    });
  };

  return (
    <article className={styles.card}>
      <p className={styles.directorate}>{serviceDirectorate}</p>
      <h3 className={styles.name}>{serviceName}</h3>
      <p className={styles.description}>{serviceDescription}</p>
      <div className={styles.footer}>
        <span className={styles.time}>{serviceProcessingTime}</span>
        <button type="button" className={styles.action} onClick={handleStart}>
          {t("dashboard.card.startRequest")}
        </button>
      </div>
    </article>
  );
}
