import { useState } from "react";
import { Dropdown } from "flowbite-react";

import WorkersAdd from "../WorkersAdd";
import DialogRemove from "../../../../components/DialogRemove";

import { FiMoreVertical as MoreIcon } from "react-icons/fi";

import Table from "../../../../components/Table";
import { TableSizes } from "../../../../models/TableSizes";
import { useMutation, useQuery } from "react-query";
import { queryClient } from "../../../../services/queryClient";
import { UserPagination } from "../../../../models/api/UserModel";
import { getWorkers, removeWorker } from "../../../../services/api/workersApi";
import { Puff } from "react-loader-spinner";
import toast from "react-hot-toast";

export default function WorkersList() {
  const [dialogMessage, setDialogMessage] = useState("");

  const [selectedWallet, setSelectedWallet] = useState("");

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [openAdd, setOpenAdd] = useState(false);
  const handleVisibilityAdd = (visibility: boolean) => {
    setOpenAdd(visibility);
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
    isLoading: isLoadingWorker,
    isError: isErrorWorker,
    error: errorWorker,
    data: workers,
  } = useQuery<UserPagination, Error>(
    ["getWorkers", page, perPage, search, startDate, endDate],
    () => getWorkers(page, perPage, search, startDate, endDate),
    { keepPreviousData: true }
  );

  const { mutate: remove } = useMutation("removeWorker", (wallet: string) => removeWorker(wallet), {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getWorkers"] }),
        toast.success("Worker removed successfully!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const title = ["Name", "Email", "Date", "Role", <MoreIcon />];
  let content: any[][] = [];

  if (isLoadingWorker) {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
      </div>
    );
  } else if (isErrorWorker) {
    return <h1 className="font-semibold text-red-600">{errorWorker.message}</h1>;
  } else {
    workers?.results?.map((data) => {
      const date = new Date(data.user.birthDate);

      content.push([
        data.user.name,
        data.user.email,
        date.toLocaleDateString(),
        data.role,
        <div>
          <Dropdown arrowIcon={false} inline={true} label={<MoreIcon />}>
            <Dropdown.Item
              onClick={() => {
                setDialogMessage(data.user.name),
                  setSelectedWallet(data.user.walletAddress),
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
    content.push(["No workers found"]);
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
        placeholder="Search Worker by Name or Email"
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
                      queryClient.refetchQueries({ queryKey: ["getWorkers"] });
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
                      queryClient.refetchQueries({ queryKey: ["getWorkers"] });
                  }}
                />
              </div>
            </div>
          </>
        }
      />

      <WorkersAdd open={openAdd} handleVisibility={handleVisibilityAdd} />
      <DialogRemove
        message={`Are you sure you want to remove ${dialogMessage} from the workers list?`}
        open={openRemove}
        handleVisibility={handleVisibilityRemove}
        actionButtonYes={() => {
          remove(selectedWallet), handleVisibilityRemove(false);
        }}
        actionButtonCancel={() => handleVisibilityRemove(false)}
      />
    </div>
  );
}
