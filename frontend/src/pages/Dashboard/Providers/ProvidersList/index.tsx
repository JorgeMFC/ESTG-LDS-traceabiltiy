import { useState } from "react";
import { Dropdown } from "flowbite-react";

import ProvidersAdd from "../ProvidersAdd";
import ProvidersEdit from "../ProvidersEdit";
import DialogRemove from "../../../../components/DialogRemove";

import { FiMoreVertical as MoreIcon } from "react-icons/fi";

import Table from "../../../../components/Table";
import { TableSizes } from "../../../../models/TableSizes";
import { Provider, ProviderPagination } from "../../../../models/api/Provider";
import { useMutation, useQuery } from "react-query";
import { getProviders, removeProvider } from "../../../../services/api/providersApi";
import { Puff } from "react-loader-spinner";
import { queryClient } from "../../../../services/queryClient";
import toast from "react-hot-toast";
import {
  getComponentsWithoutProvider,
  getOwnComponents,
} from "../../../../services/api/componentsApi";
import { Component } from "../../../../models/api/Component";

export default function ProvidersList() {
  const [dialogMessage, setDialogMessage] = useState("");
  const [currentProvider, setCurrentProvider] = useState<Provider>({
    id: "",
    organizationId: 0,
    name: "",
    email: "",
    createdAt: "",
    deletedAt: null,
  });

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

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
  const [perPage, setPerPage] = useState(10);
  const [search, setSearch] = useState("");
  let maxPage = 1;

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
    isLoading: isLoadingProvider,
    isError: isErrorProvider,
    error: errorProvider,
    data: providers,
  } = useQuery<ProviderPagination, Error>(
    ["getProviders", page, perPage, search, startDate, endDate],
    () => getProviders(page, perPage, search, startDate, endDate),
    { keepPreviousData: true }
  );

  const {
    isLoading: isLoadingComponent,
    isError: isErrorComponent,
    error: errorComponent,
    data: componentsWithoutProvider,
  } = useQuery<Array<Component>, Error>(
    ["getComponentsWithoutProvider"],
    () => getComponentsWithoutProvider(),
    { keepPreviousData: true }
  );

  const {
    isLoading: isLoadingOwnComponent,
    isError: isErrorOwnComponent,
    error: errorOwnComponent,
    data: ownComponents,
  } = useQuery<Array<Component>, Error>(
    ["getOwnComponents", currentProvider.id],
    () => getOwnComponents(currentProvider.id),
    { keepPreviousData: true }
  );

  const { mutate: remove } = useMutation("removeProvider", (id: string) => removeProvider(id), {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getProviders"] }),
        toast.success("Provider removed successfully!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const title = ["Name", "Email", "Data", <MoreIcon />];
  let content: any[][] = [];

  if (isLoadingProvider) {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
      </div>
    );
  } else if (isErrorProvider) {
    return <h1 className="font-semibold text-red-600">{errorProvider.message}</h1>;
  } else {
    providers?.results?.map((provider) => {
      const date = new Date(provider.createdAt);

      content.push([
        provider.name,
        provider.email,
        date.toLocaleDateString(),
        <div>
          <Dropdown arrowIcon={false} inline={true} label={<MoreIcon />}>
            <Dropdown.Item
              onClick={() => {
                setDialogMessage(provider.name),
                  setCurrentProvider(provider),
                  handleVisibilityEdit(true);
              }}
            >
              Edit Provider
            </Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item
              onClick={() => {
                setDialogMessage(provider.name),
                  setCurrentProvider(provider),
                  handleVisibilityRemove(true);
              }}
            >
              Remove
            </Dropdown.Item>
          </Dropdown>
        </div>,
      ]);
    });
  }

  if (content.length == 0) {
    content.push(["No providers found"]);
  }

  return (
    <div className="flex justify-center">
      <Table
        size={TableSizes.SMALL}
        titles={title}
        content={content}
        page={page}
        maxPage={maxPage}
        nextPage={nextPage}
        previousPage={previousPage}
        buttonAdd={() => handleVisibilityAdd(true)}
        setSearch={(search: string) => {
          setSearch(search);
        }}
        placeholder="Search Users by Name or Email"
        changeNumberRows={(value) => {
          setPerPage(+value);
        }}
        filters={
          <>
            <div>
              <label className="block text-sm font-medium text-gray-700">Initial Date</label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="date"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Initial Date"
                  value={startDate}
                  onChange={(e) => {
                    setStartDate(e.target.value),
                      queryClient.refetchQueries({ queryKey: ["getProviders"] });
                  }}
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">End Date</label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="date"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="End Date"
                  value={endDate}
                  onChange={(e) => {
                    setEndDate(e.target.value),
                      queryClient.refetchQueries({ queryKey: ["getProviders"] });
                  }}
                />
              </div>
            </div>
          </>
        }
      />

      <ProvidersAdd
        components={componentsWithoutProvider || []}
        open={openAdd}
        handleVisibility={handleVisibilityAdd}
      />
      <ProvidersEdit
        provider={currentProvider}
        ownComponents={(ownComponents && ownComponents) || []}
        components={componentsWithoutProvider || []}
        open={openEdit}
        handleVisibility={handleVisibilityEdit}
      />
      <DialogRemove
        message={`Are you sure you want to remove ${dialogMessage} from the providers list?`}
        open={openRemove}
        handleVisibility={handleVisibilityRemove}
        actionButtonYes={() => {
          remove(currentProvider.id), handleVisibilityRemove(false);
        }}
        actionButtonCancel={() => handleVisibilityRemove(false)}
      />
    </div>
  );
}
