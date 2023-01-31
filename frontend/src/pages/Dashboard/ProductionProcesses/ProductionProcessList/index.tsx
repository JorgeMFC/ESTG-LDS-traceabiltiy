import { Dropdown } from "flowbite-react"
import { useState } from "react"

import DialogRemove from "../../../../components/DialogRemove"
import ProductionProcessAdd from "../ProductionProcessAdd"
import ProductionProcessEdit from "../ProductionProcessEdit"

import { FiMoreVertical as MoreIcon } from 'react-icons/fi';

import toast from "react-hot-toast"
import { Puff } from "react-loader-spinner"
import { useMutation, useQuery } from "react-query"
import Table from "../../../../components/Table"
import { TableSizes } from "../../../../models/TableSizes"
import { StagePaginated } from "../../../../models/api/Stage"
import { StagesModel, StagesModelPaginated } from "../../../../models/api/StagesModel"
import { getStages } from "../../../../services/api/stageApi"
import { getStagesModel, removeStagesModel } from "../../../../services/api/stagesModelApi"
import { queryClient } from "../../../../services/queryClient"

export default function ProductionProcessList() {
	const [search, setSearch] = useState("")
	const [perPage, setPerPage] = useState(10)
	const [page, setPage] = useState(1)

	const [openAdd, setOpenAdd] = useState(false)
	const handleVisibilityAdd = (visibility: boolean) => {
		setOpenAdd(visibility)
	}

  const [openEdit, setOpenEdit] = useState(false);
  const handleVisibilityEdit = (visibility: boolean) => {
    setOpenEdit(visibility);
  };

  const [openRemove, setOpenRemove] = useState(false);
  const handleVisibilityRemove = (visibility: boolean) => {
    setOpenRemove(visibility);
  };

	const [currStageModel, setCurrStageModel] = useState<StagesModel>({
		id: "",
		organizationId: 0,
		stagesModelName: "",
	})

	const {
		isLoading: isLoadingStagesModel,
		isError: isErrorStagesModel,
		error: errorStagesModel,
		data: stagesModel,
	} = useQuery<StagesModelPaginated, Error>(
		["productionProcesses-page-stagesModel", page, perPage, search],
		() => getStagesModel(page, perPage, search),
		{ keepPreviousData: true }
	)

	const {
		isLoading: isLoadingStages,
		isError: isErrorStages,
		error: errorStages,
		data: stages,
	} = useQuery<StagePaginated, Error>("productionProcesses-page-stages", getStages)

	const removeProductionProcessMutation = useMutation(removeStagesModel, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("productionProcesses-page-stagesModel")
			toast.success("Production process removed successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	let maxPage = stagesModel?.totalPages || 0

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

  const title = ['Name', ''];

  let content: any[][] = [];

	if (isLoadingStagesModel) {
		return (
			<div className="w-full h-full flex justify-center items-center">
				<Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
			</div>
		)
	} else if (isErrorStagesModel) {
		return (
			<div>
				<h1 className="font-semibold text-red-600">{errorStagesModel?.message}</h1>
			</div>
		)
	} else {
		stagesModel?.results.forEach((element) => {
			content.push([
				element.stagesModelName,
				<div key={element.id}>
					<Dropdown arrowIcon={false} inline={true} label={<MoreIcon />}>
						<Dropdown.Item
							onClick={() => {
								handleVisibilityEdit(true)
								setCurrStageModel(element)
							}}
						>
							Edit
						</Dropdown.Item>
						<Dropdown.Divider />
						<Dropdown.Item
							onClick={() => {
								handleVisibilityRemove(true)
								setCurrStageModel(element)
							}}
						>
							Remove
						</Dropdown.Item>
					</Dropdown>
				</div>,
			])
		})
	}

	if (content.length == 0) {
		content.push(["No production processes found"])
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
					setSearch(search)
				}}
				placeholder="Search Production Processes by Name"
				changeNumberRows={(value) => {
					setPerPage(+value)
				}}
			/>

			<ProductionProcessAdd
				open={openAdd}
				stages={stages?.results || []}
				handleVisibility={handleVisibilityAdd}
			/>
			<ProductionProcessEdit
				stages={stages?.results || []}
				stageModel={currStageModel}
				open={openEdit}
				handleVisibility={handleVisibilityEdit}
			/>
			<DialogRemove
				message={`Are you sure you want to remove the production process ${currStageModel.stagesModelName} from the list?`}
				open={openRemove}
				handleVisibility={handleVisibilityRemove}
				actionButtonYes={() => {
					removeProductionProcessMutation.mutate(currStageModel.id)
					handleVisibilityRemove(false)
				}}
				actionButtonCancel={() => handleVisibilityRemove(false)}
			/>
		</div>
	)
}
