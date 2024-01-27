import { FC } from "react";
import classNames from "classnames";
import CloseIcon from "@mui/icons-material/Close";

import "./Snackbar.styles.scss";

type SnackbarProp = {
  message: String;
  handleOnClick: () => any;
  id?: string;
  className?: String;
};

const Snackbar: FC<SnackbarProp> = ({
  className: classes,
  message,
  handleOnClick,
  id,
}) => {
  const className = classNames("snackbar", classes);

  return (
    <div className={className} id={id}>
      <div className="snackbar__message">{message}</div>
      <CloseIcon className="snackbar__icon" onClick={handleOnClick} />
    </div>
  );
};

export default Snackbar;
