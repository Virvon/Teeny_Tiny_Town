namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers
{
    public class MarkersVisibility
    {
        private readonly BuildingMarker _buildingMarker;
        private readonly SelectFrame _selectFrame;

        private bool _isVisibilityAllowed;
        private bool _isBuildingMarkedShowed;
        private bool _isSelectFrameShowed;

        public MarkersVisibility(BuildingMarker buildingMarker, SelectFrame selectFrame)
        {
            _buildingMarker = buildingMarker;
            _selectFrame = selectFrame;

            _isVisibilityAllowed = false;
            _isBuildingMarkedShowed = false;
            _isSelectFrameShowed = false;
        }

        public void ChangeAllowedVisibility(bool value)
        {
            _isVisibilityAllowed = value;

            ChangeBuildingVisibility();
            ChangeSelectFrameVisibility();
        }

        public void SetBuildingShowed(bool value)
        {
            _isBuildingMarkedShowed = value;

            ChangeBuildingVisibility();
        }

        public void SetSelectFrameShowed(bool value)
        {
            _isSelectFrameShowed = value;

            ChangeSelectFrameVisibility();
        }

        private void ChangeBuildingVisibility()
        {
            if (_isBuildingMarkedShowed && _isVisibilityAllowed)
                _buildingMarker.Show();
            else
                _buildingMarker.Hide();
        }

        private void ChangeSelectFrameVisibility()
        {
            if (_isSelectFrameShowed && _isVisibilityAllowed)
                _selectFrame.Show();
            else
                _selectFrame.Hide("marker");
        }
    }
}
