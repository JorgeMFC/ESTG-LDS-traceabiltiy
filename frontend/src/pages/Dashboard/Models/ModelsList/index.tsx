import { ChangeEvent, useEffect, useState } from "react";
import { Button } from "flowbite-react";

import ModelsAdd from "../ModelsAdd";
import ModelsEdit from "../ModelsEdit";
import DialogRemove from "../../../../components/DialogRemove";
import { ModelCard } from "./ModelCard";
import { AiOutlinePlus } from "react-icons/ai";
import { IoOptionsOutline } from "react-icons/io5";
import { useMutation, useQuery } from "react-query";
import { Model, ModelPagination } from "../../../../models/api/Model";
import NoImage from "../../../../assets/noimage.jpg";
import {
  getComponentsReferences,
  getModels,
  modelOutOfProduction,
} from "../../../../services/api/modelApi";
import { Puff } from "react-loader-spinner";
import useDebounce from "../../../../hooks/useDebounce";
import { ColorPagination } from "../../../../models/api/Color";
import { getColors, getSizes } from "../../../../services/api/colorsSizesApi";
import { SizePagination } from "../../../../models/api/Size";
import { StagesModelPaginated } from "../../../../models/api/StagesModel";
import { getStagesModel } from "../../../../services/api/stagesModelApi";
import { Component } from "../../../../models/api/Component";
import { queryClient } from "../../../../services/queryClient";
import toast from "react-hot-toast";

export interface ComponentsReferences {
  sole: Array<Component>;
  toeCap: Array<Component>;
  tongue: Array<Component>;
  insole: Array<Component>;
  shoelace: Array<Component>;
  eyelets: Array<Component>;
  counterLining: Array<Component>;
  heel: Array<Component>;
}

export default function ModelsList() {
  const [dialogMessage, setDialogMessage] = useState("");
  const [currentModel, setCurrentModel] = useState<Model>({
    id: "",
    organizationId: 0,
    name: "",
    deletedAt: null,
    stagesModelId: "",
    modelPhoto: null,
  });

  const [openAdd, setOpenAdd] = useState(false);
  const handleVisibilityAdd = (visibility: boolean) => {
    setOpenAdd(visibility);
  };

  const [openEdit, setOpenEdit] = useState(false);
  const handleVisibilityEdit = (visibility: boolean) => {
    setOpenEdit(visibility);
  };

  const [openRemove, setOpenRemove] = useState(false);
  const handleVisibilityRemove = (visibility: boolean) => {
    setOpenRemove(visibility);
  };

  const [page, setPage] = useState(1);
  const [perPage, setPerPage] = useState(100);
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce<string>(search, 300);
  let maxPage = 1;

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setSearch(event.target.value);
  };

  useEffect(() => {
    setSearch(debouncedSearch);
  }, [debouncedSearch]);

  const previousPage = () => {
    let aux = page;
    if (aux > 1) {
      setPage(aux - 1);
    }
  };

  const nextPage = () => {
    let aux = page;
    if (aux < maxPage) {
      setPage(aux + 1);
    }
  };

  const {
    isLoading: isLoadingModel,
    isError: isErrorModel,
    error: errorModel,
    data: models,
  } = useQuery<ModelPagination, Error>(
    ["getModels", page, perPage, debouncedSearch],
    () => getModels(page, perPage, debouncedSearch),
    { keepPreviousData: true }
  );

  const {
    isLoading: isLoadingColor,
    isError: isErrorColor,
    error: errorColor,
    data: colors,
  } = useQuery<ColorPagination, Error>(["getColors"], () => getColors(), {
    keepPreviousData: true,
  });

  const {
    isLoading: isLoadingSize,
    isError: isErrorSize,
    error: errorSize,
    data: sizes,
  } = useQuery<SizePagination, Error>(["getSizes"], () => getSizes(), { keepPreviousData: true });

  const {
    isLoading: isLoadingProductionProcess,
    isError: isErrorProductionProcess,
    error: errorProductionProcess,
    data: productionProcesses,
  } = useQuery<StagesModelPaginated, Error>(
    "getProductionProcesses",
    () => getStagesModel(1, 100),
    { keepPreviousData: true }
  );

  const {
    isLoading: isLoadingComponentReference,
    isError: isErrorComponentReference,
    error: errorComponentReference,
    data: componentReference,
  } = useQuery<ComponentsReferences, Error>(
    ["getComponentsReferences"],
    () => getComponentsReferences(),
    {
      keepPreviousData: true,
    }
  );

  const modelOutProduction = useMutation(modelOutOfProduction, {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getModels"] });
      toast.success("Model is now out of production!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const [isFiltersVisible, setIsFiltersVisible] = useState(false);

  let content: any[] = [];

  if (isLoadingColor || isLoadingSize || isLoadingModel) {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
      </div>
    );
  } else if (isErrorColor) {
    return <h1 className="font-semibold text-red-600">{errorColor.message}</h1>;
  } else if (isErrorSize) {
    return <h1 className="font-semibold text-red-600">{errorSize.message}</h1>;
  } else {
    models?.results?.map((data) => {
      console.log(data.model.modelPhoto);

      content.push(
        <ModelCard
          key={data.model.id}
          name={data.model.name}
          image={data.model.modelPhoto}
          colors={data.colors}
          sizes={data.sizes}
          openEditCard={() => {
            setDialogMessage(data.model.name),
              setCurrentModel(data.model),
              handleVisibilityEdit(true);
          }}
          openRemoveCard={() => {
            setDialogMessage(data.model.name),
              setCurrentModel(data.model),
              handleVisibilityRemove(true);
          }}
        />
      );
    });
  }
  return (
    <div className="py-5">
      <div className="flex justify-between mb-4">
        <div className="flex items-center">
          <div className="flex text-center mr-10 items-center justify-center">
            <label className="truncate block mr-3 text-medium font-medium text-gray-600">All</label>

            <span className="truncate text-xs flex items-center justify-center font-semibold w-8 h-8 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
              5
            </span>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <Button
            onClick={() => handleVisibilityAdd(true)}
            color="dark"
            className="!bg-white !hover:bg-white !text-black w-10 h-10"
          >
            <AiOutlinePlus />
          </Button>
          <Button
            color="dark"
            className=" !bg-white !hover:bg-white !text-black !font-bold w-25 h-10"
            onClick={() => setIsFiltersVisible((prev) => !prev)}
          >
            <IoOptionsOutline size={20} className="mr-2" />
            <span className="text-xs">More</span>
          </Button>
        </div>
      </div>

      <div>
        {isFiltersVisible && (
          <div className="w-full grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-x-4 gap-y-2 p-4 my-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Name</label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="text"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Model name"
                  onChange={handleChange}
                />
              </div>
            </div>

            {/* <div>
                <label className="block text-sm font-medium text-gray-700">Colors</label>
                <div className="relative mt-1 rounded-md shadow-sm">
                  <MyCustomSelect name="colorsFilter" isMulti={true} options={colorsData} />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Sizes</label>
                <div className="relative mt-1 rounded-md shadow-sm">
                  <MyCustomSelect name="sizesFilter" isMulti={true} options={sizesData} />
                </div>
              </div> */}

            <div>
              <label className="block text-sm font-medium text-gray-700">Production Process</label>
              <select
                className="block w-full rounded-md mt-1 border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                defaultValue=""
              >
                <option value="" disabled>
                  Select production process
                </option>

                {productionProcesses?.results.map((option, index) => (
                  <option key={index} value={option.id}>
                    {option.stagesModelName}
                  </option>
                ))}
              </select>
            </div>
          </div>
        )}
      </div>

      <ModelsAdd
        colors={colors}
        sizes={sizes}
        productionProcesses={productionProcesses}
        componentsReferences={componentReference}
        open={openAdd}
        handleVisibility={handleVisibilityAdd}
      />
      <ModelsEdit
        colors={colors}
        sizes={sizes}
        productionProcesses={productionProcesses}
        componentsReferences={componentReference}
        currentModel={currentModel}
        open={openEdit}
        handleVisibility={handleVisibilityEdit}
      />
      <DialogRemove
        imageSrc={
          currentModel.modelPhoto
            ? `${import.meta.env.VITE_BASE_URL_API}${currentModel.modelPhoto}`
            : NoImage
        }
        message={`Are you sure you want to remove ${dialogMessage} from the models list?`}
        open={openRemove}
        handleVisibility={handleVisibilityRemove}
        actionButtonYes={() => {
          modelOutProduction.mutate(currentModel.id), handleVisibilityRemove(false);
        }}
        actionButtonCancel={() => handleVisibilityRemove(false)}
      />
      <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-6 gap-8">
        {content}
      </div>
    </div>
  );
}
