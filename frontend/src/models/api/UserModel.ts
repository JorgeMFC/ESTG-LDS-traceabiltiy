import { Pagination } from "./PaginationModel";

export interface UserPagination extends Pagination {
  results: [
    {
      user: User;
      nameOrg: string;
      role: string;
    }
  ];
}

export type User = {
  walletAddress: string;
  nonce: number;
  name: string;
  email: string;
  birthDate: string;
  createdAt: string;
  deletedAt: string | null;
  organizationId: number | null;
  userTypeId: number;
  avatar: string | null;
};
