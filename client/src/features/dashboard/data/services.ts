import type { ServiceListing } from "../types/service.types";

export const SERVICE_CATALOG: ServiceListing[] = [
  {
    id: "civil-status-renewal",
    directorate: "Civil Status Directorate",
    name: "Renew national civil ID",
    description: "Submit a renewal request for an expiring or damaged civil status ID card.",
    processingTime: "5–7 business days",
  },
  {
    id: "property-registration",
    directorate: "Real Estate Registration",
    name: "Register a property transfer",
    description: "File ownership transfer documents for residential or commercial property.",
    processingTime: "10–14 business days",
  },
  {
    id: "traffic-fine",
    directorate: "Traffic Directorate",
    name: "Pay a traffic violation",
    description: "Look up outstanding violations tied to your vehicle plate and settle them online.",
    processingTime: "Instant",
  },
  {
    id: "municipality-permit",
    directorate: "Basra Municipality",
    name: "Apply for a construction permit",
    description: "Start a permit application for renovation or new construction within city limits.",
    processingTime: "15–20 business days",
  },
  {
    id: "business-license",
    directorate: "Chamber of Commerce",
    name: "Renew a business license",
    description: "Renew an existing commercial registration for the current fiscal year.",
    processingTime: "3–5 business days",
  },
  {
    id: "utility-connection",
    directorate: "Electricity Directorate",
    name: "Request a new utility connection",
    description: "Apply to connect electricity service to a newly built or purchased property.",
    processingTime: "20–30 business days",
  },
];
