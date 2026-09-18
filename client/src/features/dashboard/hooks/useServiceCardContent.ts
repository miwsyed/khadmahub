import { useTranslationValue } from "@/shared/hooks/useTranslationValue";
import type { ServiceListing } from "../types/service.types";

export function useServiceCardContent(service: ServiceListing) {
  const { translate } = useTranslationValue();

  return {
    serviceName: translate(`dashboard.services.${service.id}.name`, service.name),
    serviceDirectorate: translate(`dashboard.services.${service.id}.directorate`, service.directorate),
    serviceDescription: translate(`dashboard.services.${service.id}.description`, service.description),
    serviceProcessingTime: translate(`dashboard.services.${service.id}.processingTime`, service.processingTime),
  };
}
