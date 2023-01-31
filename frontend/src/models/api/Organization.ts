import { Pagination } from "./PaginationModel";
import { User } from "./UserModel";

export interface OrganizationPagination extends Pagination {
  results: [
    {
      organization: Organization;
      user: User;
    }
  ];
}

export type Organization = {
  id: number;
  name: string;
  logo: string;
  email: string;
  walletAddress: string;
  regexIdClients: string | null;
  regexIdOrganizationsAddresses: string | null;
  regexIdLots: string | null;
  regexIdColors: string | null;
  regexIdCoponents: string | null;
  regexComponentsType: string | null;
  regexIdModels: string | null;
  regexIdProviders: string | null;
  regexIdSizes: string | null;
  regexIdStates: string | null;
  regexIdStatesModel: string | null;
  regexIdStatesType: string | null;
  photo: string | null;
};
