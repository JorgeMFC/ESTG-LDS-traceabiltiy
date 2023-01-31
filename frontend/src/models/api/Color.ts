import { Pagination } from "./Pagination";

export interface ColorPagination extends Pagination {
  results: [
    {
      id: string;
      organizationId: number;
      color1: string;
    }
  ];
}

export type Color = {
  id: string;
  organizationId: number;
  color1: string;
};
