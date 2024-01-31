import { FC, useState } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import ThumbUpOffAltIcon from "@mui/icons-material/ThumbUpOffAlt";
import StarsIcon from "@mui/icons-material/Stars";

import { FOLLOW_USER, SUBSCRIBE_TO, LIKE_SONG, CREATE_USER } from "../../../endpoints";

import "./UserForm.styles.scss";
import { Button } from "@mui/material";
import axios from "axios";
import useSnackbar from "../../../Hooks/useSnackbar";

type UserFormProps = {
  handleLoadUsers: () => void;
  handleLoadRecommendedSongs: () => void;
  userId: string | null;
  className?: String;
};

const UserForm: FC<UserFormProps> = ({
  className,
  handleLoadUsers,
  handleLoadRecommendedSongs,
  userId,
}) => {
  const classNames = classes("user-form", className);
  const { createSnackbar } = useSnackbar({
    message: "Success",
    errorMessage: "Failed request",
  });

  const [followUser, setFollowUser] = useState("");
  const [subscribeArtist, setSubscribeArtist] = useState("");
  const [likeSong, setLikeSong] = useState("");

  const [createUser, setCreateUser] = useState("");

  const handleFollowUser = async () => {
    if (followUser === "" || userId === null) return;
    await axios
      .post(FOLLOW_USER(userId, followUser))
      .then((res) => {
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };

  const handleSubscribeToArtist = async () => {
    if (subscribeArtist === "" || userId == null) return;
    await axios
      .post(SUBSCRIBE_TO(userId, subscribeArtist))
      .then((res) => {
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };
  
  const handleLikeSong = async () => {
    if (likeSong === "" || userId == null) return;
    await axios
      .post(LIKE_SONG(userId, likeSong))
      .then((res) => {
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };

  const handleCreateUser = async () => {
    if (createUser === "" || userId == null) return;
    await axios
      .post(CREATE_USER, { username: createUser})
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };
  

  return (
    <div className={classNames}>
      <h2 className="user-form__title">Users</h2>
      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Username"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
          onChange={(e)=>{setFollowUser(e.target.value)}}
          value={followUser}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="user-form__field__button"
          onClick={handleFollowUser}
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
          onChange={(e)=>{setSubscribeArtist(e.target.value)}}
          value={subscribeArtist}
        />
        <Button
          variant="contained"
          endIcon={<BookmarkIcon />}
          className="user-form__field__button"
          onClick={handleSubscribeToArtist}
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
          onChange={(e)=>{setLikeSong(e.target.value)}}
          value={likeSong}
        />
        <Button
          variant="contained"
          endIcon={<ThumbUpOffAltIcon />}
          className="user-form__field__button"
          onClick={handleLikeSong}
        >
          like
        </Button>
      </div>
      <div className="user-form__field">
        <Button
          variant="contained"
          className="user-form__field__button"
          endIcon={<AddIcon />}
          sx={{
            width: "100%",
            height: "50px",
          }}
          onClick={handleLoadUsers}
        >
          Get all users
        </Button>
      </div>
      <h2 className="user-form__title">Create new user</h2>
      <div className="user-form__field">
        <TextField
          variant="filled"
          label="Username"
          className="user-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
          onChange={(e)=>{setCreateUser(e.target.value)}}
          value={createUser}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="user-form__field__button"
          onClick={handleCreateUser}
        >
          create
        </Button>
      </div>
      <div className="user-form__field">
        <Button
          variant="contained"
          className="user-form__field__button"
          endIcon={<StarsIcon/>}
          startIcon ={<StarsIcon/>}
          sx={{
            width: "100%",
            height: "50px",
          }}
          onClick={handleLoadRecommendedSongs}
        >
          Get recommended songs
        </Button>
      </div>
    </div>
  );
};

export default UserForm;
