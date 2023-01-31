import { DialogSizes } from "../../models/DialogSizes"
import "./style.scss"

import * as Dialog from "@radix-ui/react-dialog"
import { CgClose as CloseIcon } from "react-icons/cg"

interface MyCustomDialogProps {
	title?: string
	open: boolean
	content: React.ReactNode
	handleVisibility: (visibility: boolean) => void
	actions?: React.ReactNode
	size?: DialogSizes
}

export default function MyCustomDialog(props: MyCustomDialogProps) {
	let size = props.size
	if (!size) {
		size = DialogSizes.MEDIUM
	}

	return (
		<Dialog.Root open={props.open} onOpenChange={props.handleVisibility}>
			<Dialog.Portal>
				<Dialog.Overlay className="DialogOverlay">
					<Dialog.Content className={`DialogContentWrapper ${size.size}`}>
						<div className="DialogContentSection">
							{props.title && (
								<div className="DialogTitleSection">
									<Dialog.Title className="text-xl font-semibold text-gray-900">
										{props.title}
									</Dialog.Title>
									<Dialog.Close asChild>
										<button
											type="button"
											className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5 ml-auto inline-flex items-center"
										>
											<CloseIcon className="w-5 h-5" />
										</button>
									</Dialog.Close>
								</div>
							)}

							<div className="DialogContentInternalSection">{props.content}</div>

							{props.actions && <div className="DialogActionsSection">{props.actions}</div>}
						</div>
					</Dialog.Content>
				</Dialog.Overlay>
			</Dialog.Portal>
		</Dialog.Root>
	)
}
