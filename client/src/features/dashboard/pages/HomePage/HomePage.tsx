import { useTranslation } from "react-i18next";
import { MainLayout } from "@/components/layout/MainLayout/MainLayout";
import { useAuth } from "@/context/AuthContext";
import { SERVICE_CATALOG } from "@/features/dashboard/data/services";
import { ServiceCard } from "@/features/dashboard/components/ServiceCard/ServiceCard";
import styles from "./HomePage.module.css";

function HomePageContent() {
  const { user } = useAuth();
  const { t } = useTranslation();
  const firstName = user?.fullName.split(" ")[0] ?? "";

  return (
    <MainLayout>
      <section className={styles.banner}>
        <div>
          <p className={styles.bannerKicker}>{t("dashboard.bannerKicker")}</p>
          <h1 className={styles.bannerTitle}>{t("dashboard.welcome", { name: firstName })}</h1>
          <p className={styles.bannerBody}>{t("dashboard.bannerBody")}</p>
        </div>
        <dl className={styles.bannerStats}>
          <div>
            <dt>{t("dashboard.openApplications")}</dt>
            <dd>0</dd>
          </div>
          <div>
            <dt>{t("dashboard.directoratesLinked")}</dt>
            <dd>{SERVICE_CATALOG.length}</dd>
          </div>
        </dl>
      </section>

      <section>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>{t("dashboard.availableServices")}</h2>
          <p className={styles.sectionSubtitle}>{t("dashboard.availableServicesSubtitle")}</p>
        </div>
        <div className={styles.grid}>
          {SERVICE_CATALOG.map((service) => (
            <ServiceCard key={service.id} service={service} />
          ))}
        </div>
      </section>
    </MainLayout>
  );
}

export const HomePage = HomePageContent;
