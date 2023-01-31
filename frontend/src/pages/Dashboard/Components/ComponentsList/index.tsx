import { useState } from 'react';

import toast from "react-hot-toast"
import { IoEye } from "react-icons/io5"
import { Puff } from "react-loader-spinner"
import { useQuery } from "react-query"
import Table from "../../../../components/Table"
import { TableSizes } from "../../../../models/TableSizes"
import { ComponentType, ComponentTypePaginated } from "../../../../models/api/ComponentType"
import { ProviderResult } from "../../../../models/api/Provider"
import { getComponentTypes } from "../../../../services/api/componentsTypeApi"
import { getProviders } from "../../../../services/api/providersApi"
import ComponentsAdd from "../ComponentsAdd"
import ComponentsEdit from "../ComponentsEdit"

export default function ComponentsList() {
	const [search, setSearch] = useState("")
	const [page, setPage] = useState(1)
	const [perPage, setPerPage] = useState(10)

	const [openAdd, setOpenAdd] = useState(false)
	const handleVisibilityAdd = (visibility: boolean) => {
		setOpenAdd(visibility)
	}

  const [openEdit, setOpenEdit] = useState(false);
  const handleVisibilityEdit = (visibility: boolean) => {
    setOpenEdit(visibility);
  };

  const [currComponentType, setCurrComponentType] = useState<ComponentType>({
    componentsType: { id: 0, componentType: '', organizationId: 0 },
    numReferences: 0,
  });

	const {
		isLoading: isLoadingProviders,
		isError: isErrorProviders,
		error: errorProviders,
		data: providers,
	} = useQuery<ProviderResult, Error>("components-page-providers", () => getProviders(1, 1000))

	const {
		isLoading: isLoadingComponentTypes,
		isError: isErrorComponentTypes,
		error: errorComponentTypes,
		data: componentTypesResult,
	} = useQuery<ComponentTypePaginated, Error>(
		["components-page-componentTypes", page, perPage, search],
		() => getComponentTypes(page, perPage, search),
		{ keepPreviousData: true }
	)

	let maxPage = componentTypesResult?.totalPages || 0

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

  const title = ['Component Type', 'References', ''];

  let content: any[][] = [];

	if (isLoadingComponentTypes) {
		return (
			<div className="w-full h-full flex justify-center items-center">
				<Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
			</div>
		)
	} else if (isErrorComponentTypes || isErrorProviders) {
		return (
			<div>
				<h1 className="font-semibold text-red-600">{errorComponentTypes?.message}</h1>
				<h1 className="font-semibold text-red-600">{errorProviders?.message}</h1>
			</div>
		)
	} else {
		componentTypesResult?.results?.forEach((element) => {
			content.push([
				element.componentsType.componentType,
				element.numReferences,
				<div
					key={element.componentsType.id}
					className="cursor-pointer text-lg text-gray-500"
					onClick={() => {
						if (element.numReferences > 0) {
							handleVisibilityEdit(true)
							setCurrComponentType(element)
						} else {
							toast.error("This component does not have any reference! You need to add one")
						}
					}}
				>
					<IoEye />
				</div>,
			])
		})
	}

	if (content.length == 0) {
		content.push(["No components found"])
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
					setSearch(search)
				}}
				buttonAdd={() => handleVisibilityAdd(true)}
				placeholder="Search components by name"
				changeNumberRows={(value) => {
					setPerPage(+value)
				}}
			/>

			<ComponentsAdd
				open={openAdd}
				handleVisibility={handleVisibilityAdd}
				componentTypes={componentTypesResult ? componentTypesResult.results : []}
				providers={providers ? providers.results : []}
			/>
			<ComponentsEdit
				open={openEdit}
				componentType={currComponentType}
				handleVisibility={handleVisibilityEdit}
				providers={providers ? providers.results : []}
			/>
		</div>
	)
}
