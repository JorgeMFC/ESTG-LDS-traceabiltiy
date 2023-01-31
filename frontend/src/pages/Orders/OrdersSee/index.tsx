
import { Button } from 'flowbite-react/lib/esm/components/Button';
import { useState } from 'react';
import { CgList } from 'react-icons/cg';
import { getRandomStagesTypeData, Stage, stagesData } from './data';
import { StagesContent } from './StagesContent';
import MyCustomDialog from '../../../components/MyCustomDialog';

interface OrdersSeeProps {
  open: boolean;
  handleVisibility: (visibility: boolean) => void;
}

interface DialogContentProps {
  stages: Array<Stage>;
  stagesType: Array<Stage>;
  setStagesType: React.Dispatch<React.SetStateAction<any>>;
}

const DialogContent = (props: DialogContentProps) => {
  return (
    <div className="space-y-7">
      <div className="flex justify-between w-full">
        <div className="flex space-x-2">
          <div id="imgSide">
            <div className="flex flex-wrap h-14 w-14 bg-gray-200 rounded">
              <img
                src="https://namorarte.com/image/cache/catalog/Sapato%20Viana/4-550x550.png"
                alt="..."
                className="object-cover shadow rounded max-w-full h-auto align-middle border-none"
              />
            </div>
          </div>
          <div id="contentSide" className="space-y-2">
            <h1 className="text-sm font-bold text-gray-600">Viana Shoe</h1>
            <h1 className="text-xs text-gray-400">0D129387</h1>
          </div>
        </div>
        <div className="h-7 w-7 bg-gray-800 rounded flex items-center justify-center cursor-pointer">
          <CgList size={20} color="white" />
        </div>
      </div>
      <hr />

      <div className="flex flex-col md:flex-row w-full">
        <div
          id="left-section"
          className="w-full md:w-1/2 flex justify-center md:justify-start md:ml-10"
        >
          <div className="flex">
            <StagesContent
              stages={props.stages}
              setStagesType={props.setStagesType}
            />
          </div>
        </div>

        <div
          id="right-section"
          className="w-full md:w-1/2 bg-gray-100 space-y-8 rounded-lg"
        >
          <h1 className="text-xl font-bold p-2">Shipping</h1>
          <div className="flex justify-center">
            <StagesContent stages={props.stagesType} />
          </div>
        </div>
      </div>
    </div>
  );
};

interface DialogActionsProps {
  handleVisibility: (visibility: boolean) => void;
}

const DialogActions = (props: DialogActionsProps) => {
  return (
    <div className="flex justify-end space-x-6">
      <Button
        color="dark"
        size="md"
        className="m-2"
        onClick={() => props.handleVisibility(false)}
      >
      Close
      </Button>
    </div>
  );
};

export default function OrdersSee(props: OrdersSeeProps) {
  const [stagesType, setStagesType] = useState(getRandomStagesTypeData());

  return (
    <MyCustomDialog
      open={props.open}
      handleVisibility={props.handleVisibility}
      content={
        <DialogContent
          stages={stagesData}
          stagesType={stagesType}
          setStagesType={setStagesType}
        />
      }
      actions={<DialogActions handleVisibility={props.handleVisibility} />}
    />
  );
}

