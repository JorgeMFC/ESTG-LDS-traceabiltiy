import { Color } from "./Color";
import { Pagination } from "./PaginationModel";
import { Size } from "./Size";

export interface ModelPagination extends Pagination {
  results: [
    {
      model: Model;
      colors: Array<Color>;
      sizes: Array<Size>;
    }
  ];
}

export type Model = {
  id: string;
  organizationId: number;
  name: string;
  deletedAt: string | null;
  stagesModelId: string;
  modelPhoto: string | null;
};
