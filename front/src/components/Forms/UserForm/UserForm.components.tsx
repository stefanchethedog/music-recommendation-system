import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import ThumbUpOffAltIcon from "@mui/icons-material/ThumbUpOffAlt";
import StarsIcon from "@mui/icons-material/Stars";

import "./UserForm.styles.scss";
import { Button } from "@mui/material";

type UserFormProps = {
  className?: String;
};

const UserForm: FC<UserFormProps> = ({ className }) => {
  const classNames = classes("user-form", className);
  return (
    <div className={classNames}>
      <h2 className="user-form__title">Users</h2>
      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Username"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="user-form__field__button"
        >
          follow
        </Button>
      </div>

      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Artist"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px" }}
        />
        <Button
          variant="contained"
          endIcon={<BookmarkIcon />}
          className="user-form__field__button"
        >
          subscribe
        </Button>
      </div>
      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Song"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "250px" }}
        />
        <Button
          variant="contained"
          endIcon={<ThumbUpOffAltIcon />}
          className="user-form__field__button"
        >
          like
        </Button>
      </div>
      <div className="user-form__field">
        <Button
          variant="contained"
          className="user-form__field__button"
          endIcon={<StarsIcon />}
          startIcon={<StarsIcon />}
          sx={{
            width: "100%",
            height: "50px",
          }}
        >
          Get recommended songs
        </Button>
      </div>
      <h2 className="user-form__title">Create new user</h2>
      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Username"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="user-form__field__button"
        >
          create
        </Button>
      </div>
    </div>
  );
};

export default UserForm;
