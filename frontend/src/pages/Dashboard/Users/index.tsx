import { useState } from "react";
import { MdOutlineBlock } from "react-icons/md";
import { IoMdUnlock } from "react-icons/io";
import DialogRemove from "../../../components/DialogRemove";

import Table from "../../../components/Table";
import { TableSizes } from "../../../models/TableSizes";
import { useMutation, useQuery } from "react-query";
import { queryClient } from "../../../services/queryClient";
import { UserPagination } from "../../../models/api/UserModel";
import { UserTypeModel } from "../../../models/api/UserTypeModel";
import { getUsers, blockUser, unblockUser } from "../../../services/api/usersApi";
import { getUserTypes } from "../../../services/api/usersTypeApi";
import { Puff } from "react-loader-spinner";
import toast from "react-hot-toast";
import { OrganizationPagination } from "../../../models/api/Organization";
import { getOrganizations } from "../../../services/api/organizationApi";

export default function UsersList() {
  const [dialogMessage, setDialogMessage] = useState("");

  const [selectedWallet, setSelectedWallet] = useState("");

  const [openBlock, setOpenBlock] = useState(false);
  const handleVisibilityBlock = (visibility: boolean) => {
    setOpenBlock(visibility);
  };

  const [openUnblock, setOpenUnblock] = useState(false);
  const handleVisibilityUnblock = (visibility: boolean) => {
    setOpenUnblock(visibility);
  };

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [role, setRole] = useState(0);
  const [organization, setOrganization] = useState(0);

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
    isLoading: isLoadingUser,
    isError: isErrorUser,
    error: errorUser,
    data: users,
  } = useQuery<UserPagination, Error>(
    ["getUsers", page, perPage, search, role, organization, startDate, endDate],
    () => getUsers(page, perPage, search, role, organization, startDate, endDate),
    { keepPreviousData: true }
  );

  const {
    isLoading: isLoadingUserTypes,
    isError: isErrorUserTypes,
    error: errorUserTypes,
    data: userTypes,
  } = useQuery<Array<UserTypeModel>, Error>("getUserTypes", () => getUserTypes());

  const {
    isLoading: isLoadingOrganizations,
    isError: isErrorOrganizations,
    error: errorOrganizations,
    data: organizations,
  } = useQuery<OrganizationPagination, Error>("getOrganizations", () => getOrganizations());

  const { mutate: userBlock } = useMutation("blockUser", (wallet: string) => blockUser(wallet), {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getUsers"] }),
        toast.success("User blocked successfully!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const { mutate: userUnblock } = useMutation(
    "unblockUser",
    (wallet: string) => unblockUser(wallet),
    {
      onSuccess: (response) => {
        queryClient.invalidateQueries({ queryKey: ["getUsers"] }),
          toast.success("User unblocked successfully!");
      },
      onError: (error: Error) => {
        toast.error(error.message);
      },
    }
  );

  const title = ["Name", "Email", "Date", "Role", "Organization", ""];
  let content: any[][] = [];

  if (isLoadingUserTypes) {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
      </div>
    );
  } else if (isErrorUserTypes) {
    return <h1 className="font-semibold text-red-600">{errorUserTypes.message}</h1>;
  } else if (isErrorUser) {
    return <h1 className="font-semibold text-red-600">{errorUser.message}</h1>;
  } else {
    users?.results?.map((data) => {
      const date = new Date(data.user.birthDate);
      let icon;
      if (data.user.deletedAt == null) {
        icon = (
          <MdOutlineBlock
            className="cursor-pointer"
            onClick={() => {
              handleVisibilityBlock(true),
                setDialogMessage(data.user.name),
                setSelectedWallet(data.user.walletAddress);
            }}
          />
        );
      } else {
        icon = (
          <IoMdUnlock
            className="cursor-pointer"
            onClick={() => {
              handleVisibilityUnblock(true),
                setDialogMessage(data.user.name),
                setSelectedWallet(data.user.walletAddress);
            }}
          />
        );
      }

      if (userTypes) {
        content.push([
          data.user.name,
          data.user.email,
          date.toLocaleDateString(),
          data.role,
          data.nameOrg,
          icon,
        ]);
      }
    });
    if (users) {
      maxPage = users.totalPages;
    }
  }

  if (content.length == 0) {
    content.push(["No users found"]);
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
                      queryClient.refetchQueries({ queryKey: ["getUsers"] });
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
                      queryClient.refetchQueries({ queryKey: ["getUsers"] });
                  }}
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Role</label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <select
                  className="block w-full rounded-md mb-2 border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  value={role}
                  onChange={(e) => {
                    setRole(parseInt(e.target.value)),
                      queryClient.refetchQueries({ queryKey: ["getUsers"] });
                  }}
                >
                  <option value={0}>All Roles</option>
                  {userTypes?.map((type, index) => (
                    <option key={index} value={type.id}>
                      {type.userType}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Organization</label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <select
                  className="block w-full rounded-md mb-2 border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  value={organization}
                  onChange={(e) => {
                    setOrganization(parseInt(e.target.value)),
                      queryClient.refetchQueries({ queryKey: ["getUsers"] });
                  }}
                >
                  <option value={0}>All Organizations</option>
                  {organizations?.results?.map((data, index) => (
                    <option key={index} value={data.organization.id}>
                      {data.organization.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>
          </>
        }
      />

      <DialogRemove
        message={`Are you sure you want to block ${dialogMessage}?`}
        open={openBlock}
        handleVisibility={handleVisibilityBlock}
        actionButtonYes={() => {
          handleVisibilityBlock(false), userBlock(selectedWallet);
        }}
        actionButtonCancel={() => handleVisibilityBlock(false)}
      />
      <DialogRemove
        message={`Are you sure you want to unblock ${dialogMessage}?`}
        open={openUnblock}
        handleVisibility={handleVisibilityUnblock}
        actionButtonYes={() => {
          handleVisibilityUnblock(false), userUnblock(selectedWallet);
        }}
        actionButtonCancel={() => handleVisibilityUnblock(false)}
      />
    </div>
  );
}
